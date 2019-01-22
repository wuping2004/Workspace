// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace VotingWeb.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.ApplicationInsights;

    public class HomeController : Controller
    {
        private TelemetryClient telemetry = new TelemetryClient();

        public IActionResult Index()
        {
            string datetime = System.DateTime.Now.ToString();
            telemetry.TrackEvent($"Access index page at {datetime}!");

            telemetry.TrackEvent($"Access index page finished at {datetime}!");

            return this.View();
        }

        public IActionResult Error()
        {
            return this.View();
        }
    }
}