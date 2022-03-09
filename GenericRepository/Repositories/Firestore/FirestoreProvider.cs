using GenericRepository.Models.Firestore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Google.Cloud.Firestore;
using System.Linq;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using System.Dynamic;

namespace GenericRepository.Repositories.Firestore
{
    public class FirestoreProvider
    {
        private readonly FirestoreDb _fireStoreDb = null!;

        public FirestoreProvider(FirestoreDb fireStoreDb)
        {
            _fireStoreDb = fireStoreDb;
        }

        public async Task AddOrUpdate<T>(T entity, CancellationToken ct) where T : IFirebaseEntity
        {
            var document = _fireStoreDb.Collection($"{typeof(T).Name}s").Document(entity.Id);
            await document.SetAsync(entity, cancellationToken: ct);
        }
        public async Task AddOrUpdate<T>(string collectionName,T entity, CancellationToken ct) where T : IFirebaseEntity
        {
            var document = _fireStoreDb.Collection(collectionName).Document(entity.Id);
            await document.SetAsync(entity, cancellationToken: ct);
        }

        public async Task<T> Get<T>(string id, CancellationToken ct) where T : IFirebaseEntity
        {
            dynamic obj = new ExpandoObject();
            var document = _fireStoreDb.Collection($"{typeof(T).Name}s").Document(id);
            IAsyncEnumerable<CollectionReference> subcollections = document.ListCollectionsAsync();
            IAsyncEnumerator<CollectionReference> subcollectionsEnumerator = subcollections.GetAsyncEnumerator(default);
            var snapshot = await document.GetSnapshotAsync(ct);
            obj = snapshot.ConvertTo<ExpandoObject>();
            obj.Id = snapshot.Id;
            while (await subcollectionsEnumerator.MoveNextAsync())
            {
                CollectionReference subcollectionRef = subcollectionsEnumerator.Current;
                var subCollectionDocuments = await subcollectionRef.GetSnapshotAsync();
                var subCollectionSnapshots = subCollectionDocuments.Documents.Select(doc =>
                {
                    dynamic obj2 = doc.ConvertTo<ExpandoObject>();
                    obj2.Id = doc.Id;
                    return obj2;
                });
                ((IDictionary<string, object>)obj)[subcollectionRef.Id] = subCollectionSnapshots;
            }
            var newObject = JsonConvert.SerializeObject(obj);
            return JsonConvert.DeserializeObject<T>(newObject);
        }
        public async Task<T> Get<T>(string collectionName,string id, CancellationToken ct) where T : IFirebaseEntity
        {
            dynamic obj = new ExpandoObject();
            var document = _fireStoreDb.Collection(collectionName).Document(id);
            IAsyncEnumerable<CollectionReference> subcollections = document.ListCollectionsAsync();
            IAsyncEnumerator<CollectionReference> subcollectionsEnumerator = subcollections.GetAsyncEnumerator(default);
     
            var snapshot = await document.GetSnapshotAsync(ct);

            obj = snapshot.ConvertTo<ExpandoObject>();
            obj.Id = snapshot.Id;
            while (await subcollectionsEnumerator.MoveNextAsync())
            {
                CollectionReference subcollectionRef = subcollectionsEnumerator.Current;
                var subCollectionDocuments = await subcollectionRef.GetSnapshotAsync();
                var subCollectionSnapshots = subCollectionDocuments.Documents.Select(doc =>
                {
                    dynamic obj2 = doc.ConvertTo<ExpandoObject>();
                    obj2.Id = doc.Id;
                    return obj2;
                });
                ((IDictionary<string, object>)obj)[subcollectionRef.Id] = subCollectionSnapshots;
                //obj[subcollectionRef.Id] = subCollectionSnapshots;
                //obj[subcollectionRef.Id] = subCollectionSnapshots.ToAsyncEnumerable();
                //obj[subcollectionRef.Id] = subCollectionDocuments.Documents.Select(doc =>
                //{
                //    var obj = doc.ConvertTo<ExpandoObject>();
                //    obj.Id = doc.Id;
                //    return obj;
                //}).ToList();
            }
            var newObject = JsonConvert.SerializeObject(obj);
            return JsonConvert.DeserializeObject<T>(newObject);
            //return JsonConvert.DeserializeObject<T>(((object)obj).ToString());
        }

        public async Task<DocumentSnapshot> GetCollection(string collectionName, string id, CancellationToken ct)
        {
            var document = _fireStoreDb.Collection(collectionName).Document(id);
            var snapshot = await document.GetSnapshotAsync(ct);
            return snapshot;
        }

        //public async Task<IReadOnlyCollection<T>> GetAll<T>(CancellationToken ct) where T : IFirebaseEntity
        //{
        //    var collection = _fireStoreDb.Collection($"{typeof(T).Name}s");
        //    var documents = collection.ListDocumentsAsync();
        //    return (IReadOnlyCollection<T>)await documents.Select(async doc =>
        //    {
        //        dynamic obj = new ExpandoObject();
        //        IAsyncEnumerable<CollectionReference> subcollections = doc.ListCollectionsAsync();
        //        IAsyncEnumerator<CollectionReference> subcollectionsEnumerator = subcollections.GetAsyncEnumerator(default);
        //        var snapshot = await doc.GetSnapshotAsync(ct);
        //        obj = snapshot.ConvertTo<ExpandoObject>();
        //        obj.Id = snapshot.Id;
        //        while (await subcollectionsEnumerator.MoveNextAsync())
        //        {
        //            CollectionReference subcollectionRef = subcollectionsEnumerator.Current;
        //            var subCollectionDocuments = await subcollectionRef.GetSnapshotAsync();
        //            var subCollectionSnapshots = subCollectionDocuments.Documents.Select(doc =>
        //            {
        //                dynamic obj2 = doc.ConvertTo<ExpandoObject>();
        //                obj2.Id = doc.Id;
        //                return obj2;
        //            });
        //            ((IDictionary<string, object>)obj)[subcollectionRef.Id] = subCollectionSnapshots;
        //        }
        //        var newObject = JsonConvert.SerializeObject(obj);
        //        return JsonConvert.DeserializeObject<T>(newObject);
        //    }).ToListAsync(cancellationToken: ct);
        //}
        public async Task<IReadOnlyCollection<T>> GetAll<T>(CancellationToken ct) where T : IFirebaseEntity
        {
            var collection = _fireStoreDb.Collection($"{typeof(T).Name}s");
            var snapshot = await collection.GetSnapshotAsync(ct);
            return snapshot.Documents.Select(doc =>
            {
                T obj = doc.ConvertTo<T>();
                obj.Id = doc.Id;
                return obj;
            }).ToList();
        }
        //public async Task<IReadOnlyCollection<T>> GetAll<T>(string collectionName,CancellationToken ct) where T : IFirebaseEntity
        //{
        //    var collection = _fireStoreDb.Collection(collectionName);
        //    var snapshot = await collection.GetSnapshotAsync(ct);
        //    return snapshot.Documents.Select(doc =>
        //    {
        //        T obj = doc.ConvertTo<T>();
        //        obj.Id = doc.Id;
        //        return obj;
        //    }).ToList();
        //}
        public async Task<IReadOnlyCollection<T>> GetAll<T>(string collectionName, CancellationToken ct) where T : IFirebaseEntity
        {
            var collection = _fireStoreDb.Collection(collectionName);
            var documents = collection.ListDocumentsAsync();
            IAsyncEnumerator<DocumentReference> documentsEnumerator = documents.GetAsyncEnumerator(default);
            List<T> array = new(); 
            while (await documentsEnumerator.MoveNextAsync())
            {
                try
                {
                    
                    DocumentReference currentDocument = documentsEnumerator.Current;
                    dynamic obj = new ExpandoObject();
                    IAsyncEnumerable<CollectionReference> subcollections = currentDocument.ListCollectionsAsync();
                    IAsyncEnumerator<CollectionReference> subcollectionsEnumerator = subcollections.GetAsyncEnumerator(default);
                    var snapshot = await currentDocument.GetSnapshotAsync(ct);
 
                    obj = snapshot.ConvertTo<ExpandoObject>();
                    if (snapshot.Exists && !string.IsNullOrEmpty(snapshot.Id)){
                        obj.id = snapshot.Id;
                        while (await subcollectionsEnumerator.MoveNextAsync())
                        {
                           
                            CollectionReference subcollectionRef = subcollectionsEnumerator.Current;
                            var subCollectionDocuments = await subcollectionRef.GetSnapshotAsync();
                            var subCollectionSnapshots = subCollectionDocuments.Documents.Select(doc =>
                            {
                                dynamic obj2 = doc.ConvertTo<ExpandoObject>();
                                obj2.id = doc.Id;
                                return obj2;
                            });
                            ((IDictionary<string, object>)obj)[subcollectionRef.Id] = subCollectionSnapshots;
                            var newObject = JsonConvert.SerializeObject(obj);
                            array.Add(JsonConvert.DeserializeObject<T>(newObject));
                        }
                    }
                }
                catch (Exception e)
                {

                }
               
            }
            return array;
        }

        public async Task<IReadOnlyCollection<T>> WhereEqualTo<T>(string fieldPath, object value, CancellationToken ct) where T : IFirebaseEntity
        {
            return await GetList<T>(_fireStoreDb.Collection($"{typeof(T).Name}s").WhereEqualTo(fieldPath, value), ct);
        }
        public async Task<IReadOnlyCollection<T>> WhereEqualTo<T>(string collectionName,string fieldPath, object value, CancellationToken ct) where T : IFirebaseEntity
        {
            return await GetList<T>(_fireStoreDb.Collection(collectionName).WhereEqualTo(fieldPath, value), ct);
        }

        // just add here any method you need here WhereGreaterThan, WhereIn etc ...
        private static async Task<IReadOnlyCollection<T>> GetList<T>(Query query, CancellationToken ct) where T : IFirebaseEntity
        {
            var snapshot = await query.GetSnapshotAsync(ct);
            return snapshot.Documents.Select(x => x.ConvertTo<T>()).ToList();
        }
    }

}
