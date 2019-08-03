namespace Azuria.Api.v1.Input.Converter
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IInputDataConverter<in T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="toConvert"></param>
        /// <returns></returns>
        string Convert(T toConvert);
    }
}