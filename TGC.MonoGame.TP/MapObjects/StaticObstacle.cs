using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.Cameras;
using TGC.MonoGame.TP.Geometries;
using TGC.MonoGame.TP.Collisions;
using System.Collections.Generic;
using BepuPhysics;
using BepuPhysics.Collidables;
using BepuPhysics.Constraints;
using BepuUtilities.Memory;
using TGC.MonoGame.TP.Physics.Bepu;
using NumericVector3 = System.Numerics.Vector3;

namespace TGC.MonoGame.TP.MapObjects
{
    public class StaticObstacle : Obstacle
    {
        public Simulation Simulation {get;set;}
        public Matrix World {get;set;}
        public Camera Camera {get;set;}
        public GeometricPrimitive GeometricPrimitive {get;set;}
        public StaticHandle StaticHandle {get;set;}


        public StaticObstacle (Matrix world, GeometricPrimitive geometricPrimitive, Simulation simulation, Camera camera)
        {
            this.Simulation = simulation;
            this.World = world;
            this.Camera = camera;
            this.GeometricPrimitive = geometricPrimitive;
            loadObstacle();
        }

        public void Render(Effect effect)
        {
            var viewProjection = Camera.View * Camera.Projection;
            effect.Parameters["World"].SetValue(World);
            effect.Parameters["InverseTransposeWorld"]?.SetValue(Matrix.Invert(Matrix.Transpose(World)));
            effect.Parameters["WorldViewProjection"]?.SetValue(World * viewProjection);
            GeometricPrimitive.Draw(effect);
        }

        private void loadObstacle()
        {
            Vector3 scale, translation;
            Quaternion rotation; 
            this.World.Decompose(out scale, out rotation , out translation);
            StaticHandle = this.Simulation.Statics.Add(new StaticDescription(new NumericVector3(translation.X, translation.Y, translation.Z),
            this.Simulation.Shapes.Add(new Box(scale.X, scale.Y, scale.Z))));   
        }
    }
}