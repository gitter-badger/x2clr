﻿// Copyright (c) 2013-2015 Jae-jun Kang
// See the file LICENSE for details.

using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;

namespace x2
{
    /// <summary>
    /// TCP/IP server link based on the SocketAsyncEventArgs pattern.
    /// </summary>
    public class AsyncTcpServer : AbstractTcpServer
    {
        private const int numConcurrentAcceptors = 16;

        private SocketAsyncEventArgs[] acceptEventArgs;

        /// <summary>
        /// Initializes a new instance of the AsyncTcpServer class.
        /// </summary>
        public AsyncTcpServer(string name)
            : base(name)
        {
            acceptEventArgs = new SocketAsyncEventArgs[numConcurrentAcceptors];

            for (int i = 0; i < numConcurrentAcceptors; ++i)
            {
                var saea = new SocketAsyncEventArgs();
                saea.Completed += OnAcceptCompleted;
                acceptEventArgs[i] = saea;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposed) { return; }

            for (int i = 0; i < numConcurrentAcceptors; ++i)
            {
                var saea = acceptEventArgs[i];
                saea.Completed -= OnAcceptCompleted;
                saea.Dispose();
            }

            acceptEventArgs = null;

            base.Dispose(disposing);
        }

        /// <summary>
        /// <see cref="AbstractTcpServer.AcceptInternal"/>
        /// </summary>
        protected override void AcceptInternal()
        {
            for (int i = 0, count = acceptEventArgs.Length; i < count; ++i)
            {
                AcceptImpl(acceptEventArgs[i]);
            }
        }

        private void AcceptImpl(SocketAsyncEventArgs e)
        {
            e.AcceptSocket = null;

            bool pending = socket.AcceptAsync(e);
            if (!pending)
            {
                OnAccept(e);
            }
        }

        // Completed event handler for AcceptAsync
        private void OnAcceptCompleted(object sender, SocketAsyncEventArgs e)
        {
            OnAccept(e);
        }

        // Completion callback for AcceptAsync
        private void OnAccept(SocketAsyncEventArgs e)
        {
            if (e.SocketError == SocketError.Success)
            {
                var clientSocket = e.AcceptSocket;

                if (!OnAcceptInternal(new AsyncTcpSession(this, clientSocket)))
                {
                    NotifySessionConnected(false, clientSocket.RemoteEndPoint);
                    clientSocket.Close();
                }
            }
            else
            {
                if (e.SocketError == SocketError.OperationAborted)
                {
                    Log.Info("{0} listening socket closed", Name);
                    return;
                }
                else
                {
                    Log.Error("{0} accept error : {1}", Name, e.SocketError);
                }
            }

            AcceptImpl(e);  // chain into the next accept
        }
    }
}
