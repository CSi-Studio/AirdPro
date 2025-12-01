namespace AirdPro.csimzMLParser.mzml
{
    public interface IReferenceList<T> where T : IReferenceableTag
    {
        T GetValidReference(T reference);
    }
}
