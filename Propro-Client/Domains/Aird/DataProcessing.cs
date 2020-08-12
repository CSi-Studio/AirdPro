using System.Collections.Generic;

namespace AirdPro.Domains.Aird
{
    /**
     * Description of any manipulation (from the first conversion to aird format until the creation of the current aird instance document) applied to the data.
     */
    public class DataProcessing
    {
        /**
         * Any additional manipulation not included elsewhere in the dataProcessing element.
         */
        List<string> processingOperations;

        public void addProcessingOperation(string processingOperation)
        {
            if (processingOperations == null)
            {
                processingOperations = new List<string>();
            }
            processingOperations.Add(processingOperation);
        }
    }
}