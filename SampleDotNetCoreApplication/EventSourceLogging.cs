using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Threading.Tasks;

namespace SampleDotNetCoreApplication
{
    [EventSource(Name = "SampleDotNetCoreWebApp", Guid = "b802d8a9-bf4a-45a3-b13e-a01b101c80f3")]
    public class EventSourceLogging : EventSource
    {
        public class Keywords
        {
            public const EventKeywords Request = (EventKeywords)1;
            //public const EventKeywords DataBase = (EventKeywords)2; -> we don't need this.
            public const EventKeywords Diagnostic = (EventKeywords)4;
            //public const EventKeywords Perf = (EventKeywords)8; -> we dont need this.
        }

        // Normally we will initialize this at the same place.
        public static EventSourceLogging Log = new EventSourceLogging("FamilyWebAPI");

        public EventSourceLogging(string eventSourceName) : base(eventSourceName)
        {

        }

        public class Tasks
        {
            public const EventTask Page = (EventTask)1;
            public const EventTask DBQuery = (EventTask)2;
        }

        [Event(1, Message = "Application Failure: {0}", Level = EventLevel.Error, Keywords = Keywords.Diagnostic)]
        public void Failure(string message) { WriteEvent(1, message); }

        [Event(2, Message = "Starting up.", Keywords = Keywords.Diagnostic, Level = EventLevel.Informational)]
        public void Startup() { WriteEvent(2); }

        [Event(3, Message = "Request came for {1} activityID={0} with body {2}", Opcode = EventOpcode.Start,
            Task = Tasks.Page, Keywords = Keywords.Request, Level = EventLevel.Informational)]
        public void RequestLog(string ID, string url, string body) { if (IsEnabled()) WriteEvent(3, ID, url, body); }


        [Event(3, Message = "Request response {1} activityID={0}", Opcode = EventOpcode.Start,
            Task = Tasks.Page, Keywords = Keywords.Request, Level = EventLevel.Informational)]
        public void RequestResponse(string ID, string response) { if (IsEnabled()) WriteEvent(3, ID, response); }
    }
}
