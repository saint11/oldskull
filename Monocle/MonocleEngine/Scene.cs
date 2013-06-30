using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace Monocle
{
    public class Scene
    {
        public bool Active = true;
        public SortedDictionary<int, Layer> Layers { get; private set; }
        public List<Entity>[] Tags { get; private set; }
        public Camera Camera { get; private set; }
        public uint FrameCounter { get; private set; }
        public bool Updating { get; private set; }
        public bool HasBegun { get; private set; }

        internal bool sortEntityList;
        private HashSet<Entity>[] tagsToRemove;
        private HashSet<Entity>[] tagsToAdd;

        public Scene()
        {
            Layers = new SortedDictionary<int, Layer>();

            Tags = new List<Entity>[Engine.TagAmount];
            tagsToRemove = new HashSet<Entity>[Engine.TagAmount];
            tagsToAdd = new HashSet<Entity>[Engine.TagAmount];
            for (int i = 0; i < Engine.TagAmount; i++)
            {
                Tags[i] = new List<Entity>();
                tagsToRemove[i] = new HashSet<Entity>();
                tagsToAdd[i] = new HashSet<Entity>();
            }

            Camera = new Camera();
        }

        public virtual void Begin() 
        {
            UpdateEntityLists();
            HasBegun = true;
            foreach (var layer in Layers)
                layer.Value.Begin();
        }

        public virtual void End() 
        {
            foreach (var layer in Layers)
                layer.Value.End();
        }

        public virtual void Update()
        {
            UpdateEntityLists();
            Updating = true;
            foreach (var layer in Layers)
                if (layer.Value.Active)
                    layer.Value.Update();
            Updating = false;

            FrameCounter++;
        }

        public virtual void Render()
        {
            foreach (var layer in Layers)
                if (layer.Value.Visible)
                    layer.Value.Render();
        }

        public virtual void HandleGraphicsReset()
        {
            foreach (var layer in Layers)
                layer.Value.HandleGraphicsReset();
        }

        public virtual void UpdateEntityLists()
        {
#if DEBUG
            if (Updating)
                throw new Exception("Updating Entity lists while the Scene is still in the Update loop");
#endif

            foreach (var layer in Layers)
                layer.Value.UpdateEntityList();

            for (int i = 0; i < Engine.TagAmount; i++)
            {
                if (tagsToRemove[i].Count > 0)
                {
                    foreach (var e in tagsToRemove[i])
                        Tags[i].Remove(e);
                    tagsToRemove[i].Clear();
                }

                if (tagsToAdd[i].Count > 0)
                {
                    foreach (var e in tagsToAdd[i])
                        Tags[i].Add(e);
                    tagsToAdd[i].Clear();
                }
            }
        }

        public bool OnInterval(int interval)
        {
            return FrameCounter % interval == 0;
        }

        public int EntityCount
        {
            get
            {
                int count = 0;
                foreach (var layer in Layers)
                    count += layer.Value.Entities.Count;
                return count;
            }
        }

        #region Layer Add/Remove

        public void SetLayer(int layerIndex, Layer layer)
        {
#if DEBUG
            if (Layers.ContainsKey(layerIndex))
                throw new Exception("Adding two Layers at the same Layer index");
#endif
            Layers[layerIndex] = layer;
            layer.Index = layerIndex;
            layer.Scene = this;
        }

        #endregion

        #region Entity Add/Remove

        public T Add<T>(T entity) where T : Entity
        {
#if DEBUG
            if (entity == null)
                throw new Exception("Adding null Entity");
            if (!Layers.ContainsKey(entity.LayerIndex))
                throw new Exception("Adding Entity with invalid Layer index (Scene does not contain a Layer at that index)");
#endif
            Layers[entity.LayerIndex].Add(entity);
            return entity;
        }

        public void Add<T>(params T[] entities) where T : Entity
        {
            foreach (var e in entities)
                Add(e);
        }

        public void Add<T>(List<T> entities) where T : Entity
        {
            foreach (var e in entities)
                Add(e);
        }

        public T Remove<T>(T entity) where T : Entity
        {
#if DEBUG
            if (entity == null)
                throw new Exception("Removing null Entity");
            if (!Layers.ContainsKey(entity.LayerIndex))
                throw new Exception("Removing Entity with invalid Layer index (Scene does not contain a Layer at that index)");
#endif
            Layers[entity.LayerIndex].Remove(entity);
            return entity;
        }

        public void Remove<T>(params T[] entities) where T : Entity
        {
            foreach (var e in entities)
                Remove(e);
        }

        public void Remove<T>(List<T> entities) where T : Entity
        {
            foreach (var e in entities)
                Remove(e);
        }

        #endregion

        #region Tagging System

        internal void TagEntityInstant(Entity entity, GameTags tag)
        {
            Tags[(int)tag].Add(entity);
        }

        internal void UntagEntityInstant(Entity entity, GameTags tag)
        {
            Tags[(int)tag].Remove(entity);
        }

        internal void TagEntity(Entity entity, GameTags tag)
        {
            tagsToAdd[(int)tag].Add(entity);
        }

        internal void UntagEntity(Entity entity, GameTags tag)
        {
            tagsToRemove[(int)tag].Add(entity);
        }

        public string DumpTagInfo()
        {
            string data = "";
            string[] tags = Enum.GetNames(typeof(GameTags));
            for (int i = 0; i < Tags.Length; i++)
                data += tags[i] + ": " + Tags[i].Count + " entities\n";
            return data;
        }

        public List<Entity> this[GameTags tag]
        {
            get
            {
                return Tags[(int)tag];
            }
        }

        public void Remove(GameTags tag)
        {
            foreach (var e in Tags[(int)tag])
                Layers[e.LayerIndex].Remove(e);
        }

        #endregion

        #region Collisions

        public bool CollideCheck(Vector2 point, GameTags tag)
        {
            foreach (var e in Tags[(int)tag])
                if (e.Collidable && e.CollideCheck(point))
                    return true;
            return false;
        }

        public bool CollideCheck(Vector2 from, Vector2 to, GameTags tag)
        {
            foreach (var e in Tags[(int)tag])
                if (e.Collidable && e.CollideLine(from, to))
                    return true;
            return false;
        }

        public bool CollideCheck(Rectangle rect, GameTags tag)
        {
            foreach (var e in Tags[(int)tag])
                if (e.Collidable && e.CollideCheck(rect))
                    return true;
            return false;
        }

        public Entity CollideFirst(Vector2 point, GameTags tag)
        {
            foreach (var e in Tags[(int)tag])
                if (e.Collidable && e.CollideCheck(point))
                    return e;
            return null;
        }

        public Entity CollideFirst(Vector2 from, Vector2 to, GameTags tag)
        {
            foreach (var e in Tags[(int)tag])
                if (e.Collidable && e.CollideLine(from, to))
                    return e;
            return null;
        }

        public Entity CollideFirst(Rectangle rect, GameTags tag)
        {
            foreach (var e in Tags[(int)tag])
                if (e.Collidable && e.CollideCheck(rect))
                    return e;
            return null;
        }

        public void CollideInto(Vector2 point, GameTags tag, List<Entity> list)
        {
            foreach (var e in Tags[(int)tag])
                if (e.Collidable && e.CollideCheck(point))
                    list.Add(e);
        }

        public void CollideInto(Vector2 from, Vector2 to, GameTags tag, List<Entity> list)
        {
            foreach (var e in Tags[(int)tag])
                if (e.Collidable && e.CollideLine(from, to))
                    list.Add(e);
        }

        public void CollideInto(Rectangle rect, GameTags tag, List<Entity> list)
        {
            foreach (var e in Tags[(int)tag])
                if (e.Collidable && e.CollideCheck(rect))
                    list.Add(e);
        }

        public List<Entity> CollideAll(Vector2 point, GameTags tag)
        {
            List<Entity> list = new List<Entity>();
            CollideInto(point, tag, list);
            return list;
        }

        public List<Entity> CollideAll(Vector2 from, Vector2 to, GameTags tag)
        {
            List<Entity> list = new List<Entity>();
            CollideInto(from, to, tag, list);
            return list;
        }

        public List<Entity> CollideAll(Rectangle rect, GameTags tag)
        {
            List<Entity> list = new List<Entity>();
            CollideInto(rect, tag, list);
            return list;
        }

        public void CollideDo(Vector2 point, GameTags tag, Action<Entity> action)
        {
            foreach (var e in Tags[(int)tag])
                if (e.Collidable && e.CollideCheck(point))
                    action(e);
        }

        public void CollideDo(Vector2 from, Vector2 to, GameTags tag, Action<Entity> action)
        {
            foreach (var e in Tags[(int)tag])
                if (e.Collidable && e.CollideLine(from, to))
                    action(e);
        }

        public void CollideDo(Rectangle rect, GameTags tag, Action<Entity> action)
        {
            foreach (var e in Tags[(int)tag])
                if (e.Collidable && e.CollideCheck(rect))
                    action(e);
        }

        public Vector2 LineCheck(Vector2 from, Vector2 to, GameTags tag, float precision)
        {
            Vector2 add = to - from;
            add.Normalize();
            add *= precision;

            int amount = (int)Math.Floor((from - to).Length() / precision);
            Vector2 prev = from;
            Vector2 at = from + add;

            for (int i = 0; i <= amount; i++)
            {
                if (CollideCheck(at, tag))
                    return prev;
                prev = at;
                at += add;
            }

            return to;
        }

        #endregion
    }
}
