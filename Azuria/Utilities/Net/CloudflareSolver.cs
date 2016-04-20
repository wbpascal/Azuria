using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Azuria.Utilities.ErrorHandling;
using JetBrains.Annotations;
using RestSharp;

namespace Azuria.Utilities.Net
{
    internal static class CloudflareSolver
    {
        #region

        internal static async Task<ProxerResult> Solve([NotNull] string response, [NotNull] CookieContainer cookies)
        {
            try
            {
                string lWierdVarMatch = new Regex("var\\s*?t,r,a,f,\\s?(\\S+?;)").Match(response).Groups[1].Value;
                string lWierdVar = lWierdVarMatch.Split('=')[0];
                string lWierdEquasion =
                    new Regex($"({lWierdVar}\\S+?);a.value = parseInt").Match(response).Groups[1].Value;
                string lScript = "var " + lWierdVarMatch + lWierdEquasion;
                int lCloudflareAnswer = Convert.ToInt32(JsEval.Eval(lScript)) + "proxer.me".Length;

                string lChallengeId = new Regex("name=\"jschl_vc\" value=\"(\\w+)\"").Match(response).Groups[1].Value;
                string lChallengePass = new Regex("name=\"pass\" value=\"(.+?)\"").Match(response).Groups[1].Value;

                if (!string.IsNullOrEmpty(lChallengeId.Trim()) || !string.IsNullOrEmpty(lChallengePass.Trim()) ||
                    lCloudflareAnswer == int.MinValue)
                    return new ProxerResult {Success = false};

                await Task.Delay(4000);

                IRestResponse lGetResult =
                    await
                        HttpUtility.GetWebRequestResponse(
                            $"https://proxer.me/cdn-cgi/l/chk_jschl?jschl_vc={lChallengeId}&pass={lChallengePass}&jschl_answer={lCloudflareAnswer}",
                            cookies,
                            new Dictionary<string, string>
                            {
                                {"Referer", "https://proxer.me/login?format=json&action=test"}
                            });

                return new ProxerResult {Success = lGetResult.StatusCode == HttpStatusCode.OK};
            }
            catch
            {
                return new ProxerResult {Success = false};
            }
        }

        #endregion
    }
}