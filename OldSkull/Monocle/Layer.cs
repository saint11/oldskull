using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.Collections;
using Microsoft.Xna.Framework;

namespace Monocle
{
    public class Layer
    {
        public bool Active;
        public bool Visible;
        public float CameraMultiplier;
        public BlendState BlendState;
        public SamplerState SamplerState;
        public Effect Effect;

        public Scene Scene { get; internal set; }
        public int Index { get; internal set; }
        public List<Entity> Entities { get; private set; }

        internal bool sortEntityList;
        private HashSet<Entity> toRemove;
        private List<Entity> toAdd;

        public Layer(BlendState blendState, SamplerState samplerState, float cameraMultiplier = 1, Effect effect = null)
        {
            BlendState = blendState;
            SamplerState = samplerState;
            CameraMultiplier = cameraMultiplier;
            Effect = effect;

            Active = Visible = true;
            Entities = new List<Entity>();
            toRemove = new HashSet<Entity>();
            toAdd = new List<Entity>();
        }

        public Layer()
            : this(BlendState.AlphaBlend, SamplerState.PointClamp)
        {

        }

        public Layer(float cameraMultiplier)
            : this(BlendState.AlphaBlend, SamplerState.PointClamp, cameraMultiplier)
        {

        }

        public Layer(Effect effect)
            : this(BlendState.AlphaBlend, SamplerState.PointClamp, 1, effect)
        {

        }

        public virtual void Begin() 
        {
            foreach (var e in Entities)
                e.SceneBegin();
        }

        public virtual void End() 
        {
            foreach (var e in Entities)
                e.SceneEnd();
        }

        public virtual void Update()
        {
            foreach (var e in Entities)
                if (e.Active)
                    e.Update();
        }

        public virtual void Render()
        {
            Draw.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState, SamplerState, DepthStencilState.None, RasterizerState.CullNone, Effect, Matrix.Lerp(Matrix.Identity, Scene.Camera.Matrix, CameraMultiplier));

            foreach (var e in Entities)
                if (e.Visible)
                    e.Render();
#if DEBUG
            if (Engine.Instance.Commands.Open)
                foreach (var e in Entities)
                    e.DebugRender();
#endif

            Draw.SpriteBatch.End();
        }

        public virtual void HandleGraphicsReset()
        {
            foreach (var e in Entities)
                e.HandleGraphicsReset();
        }

        internal void UpdateEntityList()
        {
            //Removing
            if (toRemove.Count > 0)
            {
                foreach (var e in toRemove)
                {
                    Entities.Remove(e);
                    e.Removed();
                    e.MarkedForRemoval = false;
                    e.Scene = null;
                }
                toRemove.Clear();
            }

            //Adding
            if (toAdd.Count > 0 || sortEntityList)
            {
                for (int i = 0; i < toAdd.Count; i++)
                {
                    Entities.Add(toAdd[i]);
                    toAdd[i].Scene = Scene;
                    toAdd[i].Added();
                }
                toAdd.Clear();

                sortEntityList = false;
                Entities.Sort(CompareDepth);
            }
        }

        private int CompareDepth(Entity a, Entity b)
        {
            return Math.Sign(b.Depth - a.Depth);
        }

        #region Entity Add/Remove

        internal void Add(Entity entity)
        {
#if DEBUG
            if (entity.Scene != null)
                throw new Exception("Entity added that is already in a Scene");
            if (toAdd.Contains(entity))
                throw new Exception("Adding the same Entity to the Scene twice in one frame.");
#endif

            toAdd.Add(entity);
        }

        internal void Remove(Entity entity)
        {
#if DEBUG
            if (!Entities.Contains(entity))
                throw new Exception("Removing Entity that is not in the Layer");
#endif
            toRemove.Add(entity);
            entity.MarkedForRemoval = true;
        }

        #endregion

        #region Entity Removes

        public void Remove(Predicate<Entity> matcher)
        {
            foreach (var e in Entities)
                if (matcher(e))
                    Remove(e);
        }

        public void Remove(GameTags tag)
        {
            foreach (var e in Scene[tag])
                if (e.LayerIndex == Index)
                    Remove(e);
        }

        public void Remove<T>() where T : Entity
        {
            foreach (var e in Entities)
                if (e is T)
                    Remove(e);
        }

        public void Clear()
        {
            foreach (var e in Entities)
                Remove(e);
        }

        #endregion

        #region Entity Queries

        public bool Contains<T>()
        {
            foreach (var e in Entities)
                if (e is T)
                    return true;
            return false;
        }

        public int Count<T>()
        {
            int amount = 0;
            foreach (var e in Entities)
                if (e is T)
                    amount++;
            return amount;
        }

        #endregion

        #region Entity Finding

        public Entity GetFirst()
        {
            return Entities.Count > 0 ? Entities[0] : null;
        }

        public T GetFirst<T>() where T : Entity
        {
            foreach (var e in Entities)
                if (e is T)
                    return e as T;
            return null;
        }

        public T GetFirst<T>(GameTags tag) where T : Entity
        {
            foreach (var e in Entities)
                if (e is T && e.Tags.Contains(tag))
                    return e as T;
            return null;
        }

        public Entity GetFirst(GameTags tag)
        {
            return GetFirst<Entity>(tag);
        }

        public List<Entity> GetList(List<Entity> list)
        {
            list.AddRange(Entities);
            return list;
        }

        public List<T> GetList<T>(List<T> list) where T : Entity
        {
            foreach (var e in Entities)
                if (e is T)
                    list.Add(e as T);
            return list;
        }

        public List<T> GetList<T>() where T : Entity
        {
            return GetList<T>(new List<T>());
        }

        public List<T> GetList<T>(GameTags tag, List<T> list) where T : Entity
        {
            foreach (var e in Entities)
                if (e is T && e.Tags.Contains(tag))
                    list.Add(e as T);
            return list;
        }

        public List<T> GetList<T>(GameTags tag) where T : Entity
        {
            return GetList<T>(tag, new List<T>());
        }

        public List<Entity> GetList(GameTags tag)
        {
            return GetList<Entity>(tag);
        }

        public List<Entity> GetList(GameTags tag, List<Entity> list)
        {
            return GetList<Entity>(tag, list);
        }

        #endregion

        #region Depth Sorting API

        public delegate int DepthSorter(Entity entity);
        static public readonly DepthSorter SortByYAscending = delegate(Entity entity) { return -(int)entity.Y; };
        static public readonly DepthSorter SortByYDescending = delegate(Entity entity) { return (int)entity.Y; };

        public void DepthSortEntities(DepthSorter depthSorter)
        {
            foreach (var entity in Entities)
                entity.depth = depthSorter(entity);
            sortEntityList = true;
        }

        public void DepthSortEntities(DepthSorter depthSorter, params Entity[] doNotSort)
        {
            foreach (var entity in Entities)
            {
                if (!doNotSort.Contains(entity))
                    entity.depth = depthSorter(entity);
            }
            sortEntityList = true;
        }

        public void DepthSortEntities(DepthSorter depthSorter, GameTags doNotSortTag)
        {
            foreach (var entity in Entities)
            {
                if (!entity.Tags.Contains(doNotSortTag))
                    entity.depth = depthSorter(entity);
            }
            sortEntityList = true;
        }

        #endregion
    }
}
