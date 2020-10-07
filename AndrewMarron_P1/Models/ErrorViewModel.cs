using System;

namespace RevatureP1.Models
{
    public class ErrorViewModel
    {
        /// <summary>
        /// Used for storing errors
        /// </summary>
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
