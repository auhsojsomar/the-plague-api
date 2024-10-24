using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace The_Plague_Api.Data.Entities
{
  public enum DiscountType
  {
    Percentage = 1,
    FixedAmount = 2
  }
}
