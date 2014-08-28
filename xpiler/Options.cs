﻿// Copyright (c) 2013, 2014 Jae-jun Kang
// See the file COPYING for license details.

using System;
using System.Collections.Generic;

namespace x2
{
    class Options
    {
        private const string DefaultSpec = "cs";

        private bool forced;
        private string outDir;
        private bool recursive;
        private string spec = DefaultSpec;

        public bool Forced { get { return forced; } }
        public string OutDir { get { return outDir; } }
        public bool Recursive { get { return recursive; } }
        public string Spec { get { return spec; } }

        static void PrintUsage()
        {
            Console.WriteLine("usage: xpiler (options) [path...]");
            Console.WriteLine(" options:");
            Console.WriteLine("  -f (--force)     : force all to be re-xpiled");
            Console.WriteLine("  -h (--help)      : print this message and quit");
            Console.WriteLine("  -o (--out-dir)   : output root directory");
            Console.WriteLine("  -r (--recursive) : process subdirectories recursively");
            Console.WriteLine("  -s (--spec) spec : specifies the target formatter");
            foreach (var pair in Xpiler.Formatters)
            {
                Console.Write("{0,18} : {1}", pair.Key, pair.Value.Description);
                if (pair.Key == DefaultSpec)
                {
                    Console.Write(" (default)");
                    Console.WriteLine();
                }
            }
        }

        public int Parse(string[] args)
        {
            var longopts = new Getopt.Option[]
            {
                new Getopt.Option("spec", Getopt.RequiredArgument, 's'),
                new Getopt.Option("out-dir", Getopt.RequiredArgument, 'o'),
                new Getopt.Option("recursive", Getopt.NoArgument, 'r'),
                new Getopt.Option("force", Getopt.NoArgument, 'f'),
                new Getopt.Option("help", Getopt.NoArgument, 'h')
            };

            var getopt = new Getopt(args, "s:o:rfh", longopts);
            while (getopt.Next() != -1)
            {
                switch (getopt.Opt)
                {
                    case 's':
                        spec = getopt.OptArg.ToLower();
                        if (!Xpiler.Formatters.ContainsKey(spec))
                        {
                            Console.Error.WriteLine(
                                "Unknown target formatter specified: {0}", spec);
                            System.Environment.Exit(1);
                        }
                        break;
                    case 'o':
                        outDir = getopt.OptArg;
                        break;
                    case 'r':
                        recursive = true;
                        break;
                    case 'f':
                        forced = true;
                        break;
                    case 'h':
                        PrintUsage();
                        System.Environment.Exit(2);
                        break;
                    default:
                        break;
                }
            }
            return getopt.OptInd;
        }
    }
}
