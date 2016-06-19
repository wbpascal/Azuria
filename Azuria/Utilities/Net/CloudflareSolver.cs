using System;
using System.Text.RegularExpressions;
using Azuria.Utilities.ErrorHandling;
using JetBrains.Annotations;

namespace Azuria.Utilities.Net
{
    internal static class CloudflareSolver
    {
        #region

        internal static ProxerResult<string> Solve([NotNull] string response, [NotNull] Uri originalUri)
        {
            try
            {
                GroupCollection lWierdEquasion =
                    new Regex(@"(var s, t, o, p[\S\s]+?};)[\S\s]+?(zyrziLd[\S\s]+?)a\.value = parseInt[\S\s]+?;").Match(
                        response).Groups;
                string lScript = "var " + lWierdEquasion[1] + lWierdEquasion[2];
                int lCloudflareAnswer = Convert.ToInt32(JsEval.Eval(lScript)) + originalUri.Host.Length;

                string lChallengeId = new Regex("name=\"jschl_vc\" value=\"(\\w+)\"").Match(response).Groups[1].Value;
                string lChallengePass = new Regex("name=\"pass\" value=\"(.+?)\"").Match(response).Groups[1].Value;

                if (string.IsNullOrEmpty(lChallengeId.Trim()) || string.IsNullOrEmpty(lChallengePass.Trim()) ||
                    lCloudflareAnswer == int.MinValue)
                    return new ProxerResult<string>(new Exception[0]);

                return
                    new ProxerResult<string>(
                        $"jschl_vc={lChallengeId}&pass={lChallengePass}&jschl_answer={lCloudflareAnswer}");
            }
            catch
            {
                return new ProxerResult<string>(new Exception[0]);
            }
        }

        #endregion
    }
}