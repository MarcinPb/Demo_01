using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace GeometryModel
{
    [Serializable]
    public class DomainObjectData
    {
        public DomainObjectData(){}

        public int ID { get; set; }
        public string Label { get; set; }
        public string Zone { get; set; }
        public bool IsActive { get; set; }
        public List<Point2D> Geometry { get; set; }
        public Dictionary<string, object> Fields { get; set; }
        public ObjectTypes ObjectType { get; set; }


        public override string ToString()
        {
            return $"{Label} ({ID})";
        }
    }
}
