using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.Infrastructure.Utility.Generates
{
    /// <summary>
    /// 序列号生成器
    /// </summary>
    public class SNComposer
    {
        private IList<NumberElement> content = null;
        private string currentResult = string.Empty;

        /// <summary>
        /// Add a number element
        /// </summary>
        /// <param name="ne">The number element to add</param>
        /// <remarks></remarks>
        public void Add(NumberElement ne)
        {
            if (content == null)
                this.content = new List<NumberElement>();
            this.content.Add(ne);
        }

        /// <summary>
        /// Calculate the value of the number that is represented by the content
        /// </summary>
        /// <returns>The SN without the sequence tail</returns>
        /// <remarks></remarks>
        public string Calculate()
        {
            this.currentResult = string.Empty;
            if (content != null && content.Count > 0)
            {
                foreach (NumberElement elem in content)
                {
                    string result = elem.Cvt.Convert(elem.Obj);
                    if (result != null)
                        this.currentResult += result;
                }
                return this.currentResult;
            }
            return null;
        }

        /// <summary>
        /// Modify a number element
        /// </summary>
        /// <param name="idx"></param>
        /// <param name="ne"></param>
        /// <remarks></remarks>
        public void Modify(int idx, NumberElement ne)
        {
            if (content == null)
                this.content = new List<NumberElement>();

            if (idx < this.content.Count)
                this.content[idx] = ne;
        }
    }
}
