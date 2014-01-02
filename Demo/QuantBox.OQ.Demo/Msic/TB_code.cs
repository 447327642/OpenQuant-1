﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

using QuantBox.OQ.Demo.Module;

namespace QuantBox.OQ.Demo.Msic
{
    public class TB_code : TargetPositionModule
    {
        FileSystemWatcher watcher;

        private void WatcherStrat(string path, string filter)
        {
            watcher = new FileSystemWatcher();
            watcher.Path = path;
            watcher.Filter = filter;

            watcher.Changed += new FileSystemEventHandler(OnProcess);

            watcher.EnableRaisingEvents = true;
        }

        private void WatcherStop()
        {
            watcher.Changed -= new FileSystemEventHandler(OnProcess);

            watcher.EnableRaisingEvents = false;
        }

        private void OnProcess(object source, FileSystemEventArgs e)
        {
            // 比较无语，保存一次文本会触发两次
            // 猜测是因为保存一次，写修改时间一次
            if (e.ChangeType == WatcherChangeTypes.Changed)
            {
                Console.WriteLine("{0},{1}", e.ChangeType, e.FullPath);

                GetLastLine(e.FullPath);
            }
        }

        public override void OnStrategyStart()
        {
            base.OnStrategyStart();

            WatcherStrat(@"E:\", string.Format("{0}.log",Instrument.Symbol));
        }

        public override void OnStrategyStop()
        {
            WatcherStop();
        }

        public string GetLastLine(string filepath)
        {
            using (StreamReader sr = new StreamReader(filepath))
            {
                string st = string.Empty;
                while (!sr.EndOfStream)
                {
                    st = sr.ReadLine();
                }

                Console.WriteLine("最后一行内容为：");
                Console.WriteLine(st);
                return st;
            }
        }

        public void ParseLine(string line)
        {

        }
    }
}


