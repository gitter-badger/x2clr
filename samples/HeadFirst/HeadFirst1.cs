using System;

using x2;
using x2.Flows;

namespace x2.Samples.HeadFirst
{
    class HeadFirst1
    {
        class CapitalizerCase : Case
        {
            protected override void SetUp()
            {
                Bind(new CapitalizeReq(), (req) => {
                    new CapitalizeResp {
                        Result = req.Message.ToUpper()
                    }.InResponseOf(req).Post();
                });
            }
        }

        class OutputCase : Case
        {
            protected override void SetUp()
            {
                Bind(new CapitalizeResp(), (e) => { Console.WriteLine(e.Result); });
            }
        }

        public static void Main()
        {
            Hub.Instance
                .Attach(new SingleThreadedFlow()
                    .Add(new CapitalizerCase())
                    .Add(new OutputCase()));

            using (var flows = new Hub.Flows())
            {
                flows.StartUp();

                while (true)
                {
                    string message = Console.ReadLine();
                    if (message == "quit")
                    {
                        break;
                    }
                    else
                    {
                        var req = new CapitalizeReq();
                        req.Message = message;
                        req.Post();
                    }
                }
            }
        }
    }
}