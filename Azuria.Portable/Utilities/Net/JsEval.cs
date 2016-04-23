using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jint;

namespace Azuria.Utilities.Net
{
    internal class JsEval
    {
        internal static string Eval(string input)
        {
            return new Engine().Execute(input).GetCompletionValue().AsNumber().ToString();
        }
    }
}
