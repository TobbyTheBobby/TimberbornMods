// using UnityEngine;
// using Steamworks;
//
// [ExecuteInEditMode]
// public class SteamWorkshopLister : MonoBehaviour
// {
//     private PublishedFileId_t[] publishedFileIds;
//
//     void Start()
//     {
//         // Initialize Steamworks API
//         SteamAPI.Init();
//
//         // Call a function to get published file IDs
//         GetPublishedItems();
//     }
//
//     void GetPublishedItems()
//     {
//         // Maximum number of items to retrieve (example: 50)
//         int maxItems = 50;
//
//         // Create a Steam call to retrieve published file IDs
//         SteamAPICall_t call = SteamUGC.CreateQueryAllUGCRequest(EUGCQuery.k_EUGCQuery_RankedByVote, EUGCMatchingUGCType.k_EUGCMatchingUGCType_Items_ReadyToUse, SteamUtils.GetAppID(), SteamUtils.GetAppID(), 1);
//         SteamUGC.SendQueryUGCRequest(call);
//
//         // Process the result when the Steam call completes
//         SteamUGC..Set((result, requestHandle) =>
//         {
//             // Check if the query was successful
//             if (result == EResult.k_EResultOK)
//             {
//                 // Get the number of results
//                 uint numResults = SteamUGC.GetQueryUGCResult(requestHandle, );
//
//                 // Retrieve the published file IDs
//                 publishedFileIds = new PublishedFileId_t[numResults];
//                 for (uint i = 0; i < numResults && i < maxItems; i++)
//                 {
//                     SteamUGCDetails_t details;
//                     if (SteamUGC.GetQueryUGCResult(requestHandle, i, out details))
//                     {
//                         publishedFileIds[i] = details.m_nPublishedFileId;
//                         Debug.Log("Published File ID: " + details.m_nPublishedFileId);
//                     }
//                 }
//             }
//             else
//             {
//                 Debug.LogError("Failed to query Steam Workshop: " + result);
//             }
//
//             // Release the Steam call handle
//             SteamUGC.ReleaseQueryUGCRequest(requestHandle);
//         });
//     }
//
//     void OnDestroy()
//     {
//         // Shutdown Steamworks API
//         SteamAPI.Shutdown();
//     }
// }