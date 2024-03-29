﻿using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.FilesProtocol
{
    public class FilesProtocolListener : IListener
    {
        private FileSystemWatcher fileWatcher;
        private CommunicatorD onConnect;

        public FilesProtocolListener()
        {
            if (!Directory.Exists(Config.SERVER_FILES_PROTOCOL_DIR))
            {
                Directory.CreateDirectory(Config.SERVER_FILES_PROTOCOL_DIR);
            }
            fileWatcher = new FileSystemWatcher(Config.SERVER_FILES_PROTOCOL_DIR);
        }

        public void Start(CommunicatorD onConnect)
        {
            this.onConnect = onConnect;
            fileWatcher.NotifyFilter = NotifyFilters.LastWrite;
            fileWatcher.Filter = "*.txt";
            fileWatcher.Changed += OnChanged;
            fileWatcher.EnableRaisingEvents = true;
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Changed)
            {
                onConnect(new FilesProtocolCommunicator(e.FullPath));
            }
        }

        public void Stop()
        {
            fileWatcher.Dispose();
        }
    }
}
