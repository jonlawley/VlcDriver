using System;
using System.Xml;

namespace VLCDriver
{
    public interface IStatusParser
    {
        void Parse();
        double Position { get; }
        string Xml { set; }
    }

    public class StatusParser : IStatusParser
    {
        public void Parse()
        {
            if (Xml == null)
            {
                throw new InvalidOperationException("Xml was not set");
            }

            var document = new XmlDocument();
            document.LoadXml(Xml);
            var root = document.DocumentElement;
            if (root == null)
            {
                throw new InvalidOperationException("The Status root element was empty");
            }
            var positionElement = root["position"];
            if (positionElement == null)
            {
                throw new InvalidOperationException("The position element was not present");
            }
            Position = Convert.ToDouble(positionElement.InnerText);
        }

        public double Position { get; private set; }

        public string Xml { get; set; }

        //TODO What about the state, we can get the actual state from this xml?
    }
}