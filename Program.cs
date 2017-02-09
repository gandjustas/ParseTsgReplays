using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Threading;
using Microsoft.EntityFrameworkCore;

namespace ParseTsgReplays
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: dotnet ParseTsgReplays.dll path-to-unpacked-replays");
                return;
            }

            var rebuildBase = false;

            var db = new ReplaysContext();
            db.Database.Migrate();
            var existingRecords = !rebuildBase ? db.Replays.AsEnumerable().Select(r => new { r.Server, Timestamp = r.Timestamp.ToString("yyyy-MM-dd-HH-mm-ss") }).ToSet() : null;
            db.Dispose();


            var queue = new BlockingCollection<Replay>();
            var task = Task.Run(() =>
            {
                var counter = 0;
                List<Replay> toAdd = new List<Replay>();
                foreach (var r in queue.GetConsumingEnumerable())
                {
                    toAdd.Add(r);
                    if (++counter % 100 == 0)
                    {
                        SaveReplayList(toAdd);
                        counter = 0;
                        toAdd.Clear();
                    }
                }
                SaveReplayList(toAdd);
            });


            var dir = args[0];
            var provider = CultureInfo.InvariantCulture;
            int counterParsed = 0;
            int counterProcessed = 0;

            var exceptions = new BlockingCollection<Tuple<string, ParseException>>();
            Task.Run(() =>
            {
                foreach (var t in exceptions.GetConsumingEnumerable())
                {
                    Console.WriteLine(t.Item1);
                    Console.WriteLine(t.Item2.ToString());
                    Console.WriteLine();
                }
            });

            Parallel.ForEach(Directory.EnumerateDirectories(dir), replayDir =>
            //foreach (var replayDir in Directory.EnumerateDirectories(dir))
            {

                var replayName = Path.GetFileName(replayDir);
                var key = new
                {
                    Server = replayName.Substring(0, 2),
                    Timestamp = replayName.Substring(3, 19)
                };
                if (!existingRecords.Contains(key))
                {
                    //Console.WriteLine(replayName);
                    using (var file = File.OpenText(Path.Combine(replayDir, "log.txt")))
                    {
                        var p = new ReplayProcessor(file);
                        Replay replay;
                        try
                        {
                            replay = p.ProcessReplay();
                            if (replay != null)
                            {
                                Interlocked.Increment(ref counterParsed);
                                //if (counter % 100 == 0)
                            }
                        }
                        catch (ParseException e)
                        {
                            exceptions.Add(Tuple.Create(replayName, e));
                            replay = p.GetResult();
                        }
                        if (replay != null)
                        {
                            replay.Server = replayName.Substring(0, 2);
                            queue.Add(replay);
                        }
                        Interlocked.Increment(ref counterProcessed);

                        if (counterProcessed % 100 == 0) Console.WriteLine("Processed {0} parsed {1}", counterProcessed, counterParsed);
                    }
                }
            });
            Console.WriteLine("Processed {0} parsed {1}", counterProcessed, counterParsed);

            queue.CompleteAdding();
            exceptions.CompleteAdding();
            task.Wait();

            ClearDuplicates();
        }

        private static void ClearDuplicates()
        {
            //Clear duplicates
            using (var db = new ReplaysContext())
            {
                db.Database.ExecuteSqlCommand(@"
                    delete   from EnterExitEvents
                    where    ReplayId not in
                             (
                             select  min(Id)
                             from    Replays
                             group by Server, Timestamp
                             );

                    delete   from Units
                    where    ReplayId not in
                             (
                             select  min(Id)
                             from    Replays
                             group by Server, Timestamp
                             );

                    delete   from Kills
                    where    ReplayId not in
                             (
                             select  min(Id)
                             from    Replays
                             group by Server, Timestamp
                             )	;	 
		 
                    delete   from Replays
                    where    Id not in
                             (
                             select  min(Id)
                             from    Replays
                             group by Server, Timestamp
                             )	;	 
                ");
            }
        }

        private static void SaveReplayList(List<Replay> toAdd)
        {
            using (var db = new ReplaysContext())
            {
                db.Replays.AddRange(toAdd);
                db.SaveChanges();
            }
        }
    }
}
