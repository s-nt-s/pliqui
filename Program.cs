/*
 * Creado por SharpDevelop.
 * Usuario: 99GU5372
 * Fecha: 28/11/2013
 * Hora: 11:58
 * 
 * Para cambiar esta plantilla use Herramientas | Opciones | Codificación | Editar Encabezados Estándar
 */
using System;
using System.Windows.Forms;
using System.Xml;

namespace pliqui
{
	/// <summary>
	/// Class with program entry point.
	/// </summary>
	internal sealed class Program
	{
		/// <summary>
		/// Program entry point.
		/// </summary>
		[STAThread]
		private static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm(args!=null && args.Length>0?args[0]:null));
		}
		
	}
}
