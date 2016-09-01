using System.Reflection;
using System.Threading.Tasks;
using Azuria.Utilities.ErrorHandling;
using Azuria.Utilities.Properties;
using JetBrains.Annotations;

namespace Azuria.Utilities.Extensions
{
    internal static class InitialisationExtensions
    {
        #region

        [ItemNotNull]
        internal static async Task<ProxerResult> InitAllInitalisableProperties(this object source)
        {
            int lFailedInits = 0, lInitialiseFunctions = 0;
            ProxerResult lReturn = new ProxerResult();
            foreach (PropertyInfo propertyInfo in source.GetType().GetRuntimeProperties())
            {
                if (!propertyInfo.PropertyType.ImplementsGenericInterface(typeof(IInitialisableProperty<>))) continue;
                lInitialiseFunctions++;
                try
                {
                    object lInitialisableObject = propertyInfo.GetMethod.Invoke(source, null);
                    ProxerResult lResult = await (Task<ProxerResult>) lInitialisableObject.GetType()
                        .GetTypeInfo()
                        .GetDeclaredMethod("FetchObject")
                        .Invoke(lInitialisableObject, null);

                    if (!lResult.Success)
                    {
                        lReturn.AddExceptions(lResult.Exceptions);
                        lFailedInits++;
                    }
                }
                catch
                {
                    lFailedInits++;
                }
            }

            if (lFailedInits < lInitialiseFunctions)
                lReturn.Success = true;

            return lReturn;
        }

        internal static bool IsFullyInitialised([NotNull] this object objectToTest)
        {
            int lInitialisedProperties = 0, lInitialiseFunctions = 0;
            foreach (PropertyInfo propertyInfo in objectToTest.GetType().GetRuntimeProperties())
                if (propertyInfo.PropertyType.ImplementsGenericInterface(typeof(IInitialisableProperty<>)))
                {
                    lInitialiseFunctions++;
                    try
                    {
                        object lPropertyObject;
                        bool lIsInitialised =
                            (bool) (lPropertyObject = propertyInfo.GetValue(objectToTest))
                                .GetType()
                                .GetTypeInfo()
                                .GetDeclaredProperty("IsInitialisedOnce").GetValue(lPropertyObject);

                        if (lIsInitialised) lInitialisedProperties++;
                    }
                    catch
                    {
                        return false;
                    }
                }

            return lInitialisedProperties == lInitialiseFunctions;
        }

        #endregion
    }
}