using System;

using BO4E.ENUM;
using BO4E.meta;

using Newtonsoft.Json;

using ProtoBuf;

namespace BO4E.COM
{
    /// <summary>Mit dieser Komponente werden Messlokationen zu Marktlokationen zugeordnet. Dabei kann eine arithmetische Operation (Addition, Subtraktion) angegeben werden, mit der die Messlokation zum Verbrauch der Marklokation beiträgt.</summary>
    [ProtoContract]
    public class Messlokationszuordnung : COM
    {
        /// <summary>Die Messlokations-ID, früher die Zählpunktbezeichnung.</summary>
        [DataCategory(DataCategory.POD)]
        [JsonProperty(PropertyName = "messlokationsId", Required = Required.Always)]
        [ProtoMember(3)]
        public string MesslokationsId { get; set; }
        /// <summary>Die Operation, mit der eine Messung an dieser Lokation für den Gesamtverbrauch der Marktlokation verrechnet wird. Beispielsweise bei einer Untermessung, wird der Verbauch der Untermessung subtrahiert. Details <see cref="ArithmetischeOperation" /></summary>
        [JsonProperty(PropertyName = "arithmetik", Required = Required.Default)] // Default weil Hochfrequenz/energy-service-hub#35
        [ProtoMember(4)]
        public ArithmetischeOperation? Arithmetik { get; set; }
        /// <summary>Zeitpunkt, ab dem die Messlokation zur Marktlokation gehört</summary>
        [JsonProperty(PropertyName = "gueltigSeit", Required = Required.Default)]
        [ProtoMember(5)]
        public DateTime? GueltigSeit { get; set; }
        /// <summary>Zeitpunkt, bis zu dem die Messlokation zur Marktlokation gehört</summary>
        [JsonProperty(PropertyName = "gueltigBis", Required = Required.Default)]
        [ProtoMember(6)]
        public DateTime? GueltigBis { get; set; }
    }
}