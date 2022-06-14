using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map
{
    public readonly List<LivingEntity>[,] map;

    readonly Vector2[,] centres;

    readonly int regionSize;
    readonly int numRegions;

    public int numEntities;

    public Map(int size, int regionSize)
    {
        this.regionSize = regionSize;

        numRegions = Mathf.CeilToInt(size / (float)regionSize);

        map = new List<LivingEntity>[numRegions, numRegions];
        centres = new Vector2[numRegions, numRegions];

        for (int y = 0; y < numRegions; y++)
        {
            for (int x = 0; x < numRegions; x++)
            {
                Coord regionBottomLeft = new Coord(x * regionSize, y * regionSize);
                Coord regionTopRight = new Coord(x * regionSize + regionSize, y * regionSize + regionSize);

                Vector2 centre = (Vector2)(regionBottomLeft + regionTopRight) / 2f;

                centres[x, y] = centre;
                map[x, y] = new List<LivingEntity>();
            }
        }
    }

    public List<LivingEntity> GetEntities(Coord origin, float viewDistance)
    {
        List<RegionInfo> visibleRegions = GetRegionsInView(origin, viewDistance);

        float sqrViewDst = viewDistance * viewDistance;
        var visibleEntities = new List<LivingEntity>();

        for (int i = 0; i < visibleRegions.Count; i++)
        {
            Coord regionCoord = visibleRegions[i].coord;

            for (int j = 0; j < map[regionCoord.x, regionCoord.y].Count; j++)
            {
                LivingEntity entity = map[regionCoord.x, regionCoord.y][j];

                float sqrDst = Coord.SqrDistance(entity.coord, origin);

                if(sqrDst > sqrViewDst)
                {
                   if(EnvironmentUtility.TileIsVisible(origin.x, origin.y, entity.coord.x, entity.coord.y))
                   {
                        visibleEntities.Add(entity);
                   }
                }
            }
        }

        return visibleEntities;
    }

    public LivingEntity ClosestEntity(Coord origin, float viewDistance)
    {
        List<RegionInfo> visibleRegions = GetRegionsInView(origin, viewDistance);
        LivingEntity closestEntity = null;

        float closestSqrDst = viewDistance * viewDistance + .01f;

        for (int i = 0; i < visibleRegions.Count; i++)
        {
            if(closestSqrDst <= visibleRegions[i].sqrDstToClosestEdge)
            {
                break;
            }

            Coord regionCoord = visibleRegions[i].coord;

            for (int j = 0; j < map[regionCoord.x, regionCoord.y].Count; j++)
            {
                LivingEntity entity = map[regionCoord.x, regionCoord.y][j];

                float sqrDst = Coord.SqrDistance(entity.coord, origin);

                if(sqrDst < closestSqrDst)
                {
                    if(EnvironmentUtility.TileIsVisible(origin.x, origin.y, entity.coord.x, entity.coord.y))
                    {
                        closestSqrDst = sqrDst;
                        closestEntity = entity;
                    }
                }
            }
        }

        return closestEntity;
    }

    public List<RegionInfo> GetRegionsInView(Coord origin, float viewDistancce)
    {
        List<RegionInfo> regions = new List<RegionInfo>();

        int originRegionX = origin.x / regionSize;
        int originRegionY = origin.y / regionSize;

        float sqrViewDst = viewDistancce * viewDistancce;

        Vector2 viewCentre = origin * Vector2.one * .5f;

        int searchNum = Mathf.Max(1, Mathf.CeilToInt(viewDistancce / regionSize));

        for (int offsetY = -searchNum; offsetY <= searchNum; offsetY++)
        {
            for (int offsetX = -searchNum; offsetX <= searchNum; offsetX++)
            {
                int viewedRegionX = originRegionX + offsetX;
                int viewedRegionY = originRegionY + offsetY;

                if(viewedRegionX >= 0 && viewedRegionX < numRegions && viewedRegionY >= 0 && viewedRegionY < numRegions)
                {
                    float ox = Mathf.Max(0, Mathf.Abs(viewCentre.x - centres[viewedRegionX, viewedRegionY].x) - regionSize / 2f);
                    float oy = Mathf.Max(0, Mathf.Abs(viewCentre.y - centres[viewedRegionX, viewedRegionY].y) - regionSize / 2f);
                    float sqrDstFromRegionEdge = ox * ox + oy * oy;

                    if(sqrDstFromRegionEdge <= sqrViewDst)
                    {
                        RegionInfo info = new RegionInfo(new Coord(viewedRegionX, viewedRegionY), sqrDstFromRegionEdge);

                        regions.Add(info);
                    }
                }
            }
        }

        regions.Sort((a, b) => a.sqrDstToClosestEdge.CompareTo(b.sqrDstToClosestEdge));

        return regions;
    }

    public void Add(LivingEntity e, Coord coord)
    {
        int regionX = coord.x / regionSize;
        int regionY = coord.y / regionSize;

        int index = map[regionX, regionY].Count;

        e.mapIndex = index;
        e.mapCoord = coord;
        map[regionX, regionY].Add(e);

        numEntities++;
    }

    public void Remove(LivingEntity e, Coord coord)
    {
        int regionX = coord.x / regionSize;
        int regionY = coord.y / regionSize;

        int index = e.mapIndex;
        int lastElementIndex = map[regionX, regionY].Count - 1;

        if(index != lastElementIndex)
        {
            map[regionX, regionY][index] = map[regionX, regionY][lastElementIndex];
            map[regionX, regionY][index].mapIndex = e.mapIndex;
        }

        map[regionX, regionY].RemoveAt(lastElementIndex);

        numEntities--;
    }

    public void Move(LivingEntity e, Coord fromCoord, Coord toCoord)
    {
        Remove(e, fromCoord);
        Add(e, toCoord);
    }

    public struct RegionInfo
    {
        public readonly Coord coord;

        public readonly float sqrDstToClosestEdge;

        public RegionInfo(Coord coord, float sqrDstToClosestEdge)
        {
            this.coord = coord;
            this.sqrDstToClosestEdge = sqrDstToClosestEdge;
        }
    }

    public void DrawDebugGizmos(Coord coord, float viewDst)
    {
        bool showViewedRegions = true;
        bool showOccupancy = false;

        float height = Environment.tileCentres[0, 0].y + .1f;

        Gizmos.color = Color.black;

        int regionX = coord.x / regionSize;
        int regionY = coord.y / regionSize;

        for(int i = 0; i <= numRegions; i++)
        {
            Gizmos.DrawLine(new Vector3(i * regionSize, height, 0), new Vector3(i * regionSize, height, regionSize * numRegions));
            Gizmos.DrawLine(new Vector3(0, height, i * regionSize), new Vector3(regionSize * numRegions, height, i * regionSize));
        }

        for (int y = 0; y < numRegions; y++)
        {
            for (int x = 0; x < numRegions; x++)
            {
                Vector3 centre = new Vector3(centres[x, y].x, height, centres[x, y].y);
                Gizmos.DrawSphere(centre, .3f);
            }
        }

        if(showViewedRegions)
        {
            List<RegionInfo> regionsInView = GetRegionsInView(coord, viewDst);

            for (int y = 0; y < numRegions; y++)
            {
                for (int x = 0; x < numRegions; x++)
                {
                    Vector3 centre = new Vector3(centres[x, y].x, height, centres[x, y].y);

                    for (int i = 0; i < regionsInView.Count; i++)
                    {
                        if(regionsInView[i].coord.x == x && regionsInView[i].coord.y == y)
                        {
                            var prevCol = Gizmos.color;

                            Gizmos.color = new Color(1, 0, 0, 1 - i / Mathf.Max(1, regionsInView.Count - 1f) * .5f);
                            Gizmos.DrawCube(centre, new Vector3(regionSize, .1f, regionSize));
                            Gizmos.color = prevCol;
                        }
                    }
                }
            }
        }

        if(showOccupancy)
        {
            int maxOccupants = 0;

            for (int y = 0; y < numRegions; y++)
            {
                for (int x = 0; x < numRegions; x++)
                {
                    Vector3 centre = new Vector3(centres[x, y].x, height, centres[x, y].y);

                    int numOccupants = map[x, y].Count;

                    if(numOccupants > 0)
                    {
                        var prevCol = Gizmos.color;

                        Gizmos.color = new Color(1, 0, 0, numOccupants / (float)maxOccupants);
                        Gizmos.DrawCube(centre, new Vector3(regionSize, .1f, regionSize));
                        Gizmos.color = prevCol;
                    }
                }
            }
        }
    }
}
