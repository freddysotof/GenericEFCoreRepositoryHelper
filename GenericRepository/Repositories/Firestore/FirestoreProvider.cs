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
            var document = _fireStoreDb.Collection($"{typeof(T).Name}s").Document(id);
            var snapshot = await document.GetSnapshotAsync(ct);
            T obj = snapshot.ConvertTo<T>();
            obj.Id = snapshot.Id;
            return obj;
        }
        public async Task<T> Get<T>(string collectionName,string id, CancellationToken ct) where T : IFirebaseEntity
        {
            var document = _fireStoreDb.Collection(collectionName).Document(id);
            var snapshot = await document.GetSnapshotAsync(ct);
            T obj = snapshot.ConvertTo<T>();
            obj.Id = snapshot.Id;
            return obj;
        }

        public async Task<DocumentSnapshot> GetCollection(string collectionName, string id, CancellationToken ct)
        {
            var document = _fireStoreDb.Collection(collectionName).Document(id);
            var snapshot = await document.GetSnapshotAsync(ct);
            return snapshot;
        }

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
        public async Task<IReadOnlyCollection<T>> GetAll<T>(string collectionName,CancellationToken ct) where T : IFirebaseEntity
        {
            var collection = _fireStoreDb.Collection(collectionName);
            var snapshot = await collection.GetSnapshotAsync(ct);
            return snapshot.Documents.Select(doc =>
            {
                T obj = doc.ConvertTo<T>();
                obj.Id = doc.Id;
                return obj;
            }).ToList();
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
