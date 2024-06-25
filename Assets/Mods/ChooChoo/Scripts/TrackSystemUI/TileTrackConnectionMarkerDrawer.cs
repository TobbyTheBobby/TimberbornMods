using Bindito.Core;
using ChooChoo.TrackSystem;
using Timberborn.BaseComponentSystem;
using Timberborn.BlockSystem;
using Timberborn.Coordinates;
using Timberborn.Core;
using Timberborn.CoreUI;
using Timberborn.Rendering;
using Timberborn.SelectionSystem;
using UnityEngine;

namespace ChooChoo.TrackSystemUI
{
    public class TileTrackConnectionMarkerDrawer : BaseComponent, ISelectionListener, IPreviewSelectionListener
    {
        private static readonly float EntranceMarkerYOffset = 0.2f;
        private static readonly float TileYOffset = 0.005f;
        private MarkerDrawerFactory _markerDrawerFactory;
        private Colors _colors;
        private MeshDrawer _tileMeshDrawer;
        private MeshDrawer _entranceMarkerMeshDrawer;
        private IBlockObjectModel _blockObjectModel;
        private TrackPiece _trackPiece;
        private bool _isPreview;
        private TrackConnection[] _uniqueTrackConnections;

        [Inject]
        public void InjectDependencies(MarkerDrawerFactory markerDrawerFactory, Colors colors)
        {
            _markerDrawerFactory = markerDrawerFactory;
            _colors = colors;
        }

        public void Awake()
        {
            enabled = false;
        }

        public void Start()
        {
            _blockObjectModel = GetComponentFast<IBlockObjectModel>();
            _trackPiece = GetComponentFast<TrackPiece>();
            _tileMeshDrawer = _markerDrawerFactory.CreateTileDrawer();
            _entranceMarkerMeshDrawer = _markerDrawerFactory.CreateEntranceMarkerDrawer();
        }

        public void LateUpdate()
        {
            if ((_blockObjectModel != null ? (_blockObjectModel.IsModelShown ? 1 : 0) : 1) == 0)
                return;

            DrawTrackConnections();
        }

        public void OnSelect()
        {
            _isPreview = false;
            enabled = true;
        }

        public void OnUnselect()
        {
            enabled = false;
        }

        public void OnPreviewSelect()
        {
            _isPreview = true;
            enabled = true;
        }

        public void OnPreviewUnselect()
        {
            enabled = false;
        }

        private void DrawTrackConnections()
        {
            foreach (var trackConnection in _trackPiece.PositionedTrackConnections)
            {
                var coordinates = trackConnection.Coordinates + trackConnection.Direction2D.ToOffset();
                var rotation = Quaternion.AngleAxis(trackConnection.Direction2D.ToAngle(), Vector3.up);
                var color = _isPreview ? _colors.PreviewLandTile : _colors.BuildingLandTile;
                _tileMeshDrawer.DrawAtCoordinates(coordinates, TileYOffset, color);
                _entranceMarkerMeshDrawer.DrawAtCoordinates(coordinates, EntranceMarkerYOffset, rotation);
            }
        }
    }
}