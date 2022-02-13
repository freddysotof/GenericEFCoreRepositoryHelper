using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GenericRepository.Models.Firestore
{
    public interface IFirebaseEntity
    {
        //[FirestoreProperty("id")]
        public string Id { get; set; }
    }

}
