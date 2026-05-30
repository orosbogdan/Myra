using AssetManagementBase;
using DigitalRiseModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myra.Graphics2D;
using Myra.Graphics2D.UI;
using System;
using System.ComponentModel;
using System.IO;

namespace Myra.Samples.CustomWidgets
{
	public class Scene3D : Widget
	{
		private const float NearPlaneDistance = 0.1f;
		private const float FarPlaneDistance = 1000.0f;
		private const float ViewAngle = 60.0f;

		private BasicEffect _basicEffect;
		private DateTime? _lastDt = null;

		[Category("3D")]
		public Color Color { get; set; } = Color.Green;

		[Category("3D")]
		public float SpecularPower { get; set; } = 50.0f;

		[Category("3D")]
		public float MeshScale { get; set; } = 2.0f;

		[Category("3D")]
		public float DegreesPerSecond { get; set; } = 10.0f;


		[Browsable(false)]
		public DrMeshPart Mesh { get; set; }

		private float RotationAngle { get; set; } = 0;

		private Texture2D Texture { get; set; }

		public Scene3D()
		{
			HorizontalAlignment = HorizontalAlignment.Stretch;
			VerticalAlignment = VerticalAlignment.Stretch;

			var assetManager = AssetManager.CreateFileAssetManager(Path.Combine(AppContext.BaseDirectory, "Assets"));
			Texture = assetManager.LoadTexture2D(MyraEnvironment.GraphicsDevice, "Textures/checker.dds");
		}

		public override void InternalRender(RenderContext context)
		{
			base.InternalRender(context);

			if (Mesh == null)
			{
				return;
			}

			var device = MyraEnvironment.GraphicsDevice;

			if (_basicEffect == null)
			{
				_basicEffect = new BasicEffect(device)
				{
					LightingEnabled = true
				};

				_basicEffect.DirectionalLight0.Enabled = true;
				_basicEffect.DirectionalLight0.Direction = new Vector3(-1, -1, -1);
				_basicEffect.DirectionalLight0.DiffuseColor = Color.White.ToVector3();
				_basicEffect.DirectionalLight0.SpecularColor = Color.White.ToVector3();
			}

			context.End();

			// Save current device state
			var oldViewPort = device.Viewport;
			var oldDepthStencilState = device.DepthStencilState;
			var oldRasterizerState = device.RasterizerState;
			var oldBlendState = device.BlendState;
			var oldSamplesState = device.SamplerStates[0];

			// Set the new one
			var screenPosition = ToGlobal(Point.Zero);
			device.Viewport = new Viewport(screenPosition.X, screenPosition.Y, ActualBounds.Width, ActualBounds.Height);

			device.DepthStencilState = DepthStencilState.Default;
			device.RasterizerState = RasterizerState.CullCounterClockwise;
			device.BlendState = BlendState.AlphaBlend;
			device.SamplerStates[0] = SamplerState.LinearWrap;

			// Set vertex/index buffers
			device.SetVertexBuffer(Mesh.VertexBuffer);
			device.Indices = Mesh.IndexBuffer;

			// Calculate and set effect params
			var view = Matrix.CreateLookAt(new Vector3(0, 0, 5), Vector3.Zero, Vector3.Up);
			var projection = Matrix.CreatePerspectiveFieldOfView(
				MathHelper.ToRadians(ViewAngle),
				device.Viewport.AspectRatio,
				NearPlaneDistance, FarPlaneDistance
			);

			var world = Matrix.CreateRotationY(MathHelper.ToRadians(RotationAngle)) * Matrix.CreateScale(MeshScale);

			_basicEffect.View = view;
			_basicEffect.World = world;
			_basicEffect.Projection = projection;

			_basicEffect.DiffuseColor = Color.ToVector3();
			_basicEffect.SpecularPower = SpecularPower;

			_basicEffect.Texture = Texture;
			_basicEffect.TextureEnabled = true;

			// Render
			foreach (EffectPass pass in _basicEffect.CurrentTechnique.Passes)
			{
				pass.Apply();

#if FNA
				device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0,
				0,
				_mesh.VertexBuffer.VertexCount,
				0,
				_mesh.PrimitiveCount);
#else
				device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, Mesh.PrimitiveCount);
#endif
			}

			// Update the rotation angle
			var now = DateTime.Now;
			if (_lastDt != null)
			{
				var passed = now - _lastDt.Value;
				var degrees = (float)passed.TotalSeconds * DegreesPerSecond;
				RotationAngle += degrees;
			}

			_lastDt = now;

			// Restore the device state
			device.Viewport = oldViewPort;
			device.DepthStencilState = oldDepthStencilState;
			device.RasterizerState = oldRasterizerState;
			device.BlendState = oldBlendState;
			device.SamplerStates[0] = oldSamplesState;

			// Don't forget to start the context again
			context.Begin();
		}
	}
}
