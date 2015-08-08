/*
 * Creado por SharpDevelop.
 * Usuario: 99GU5372
 * Fecha: 28/11/2013
 * Hora: 11:58
 * 
 * Para cambiar esta plantilla use Herramientas | Opciones | Codificación | Editar Encabezados Estándar
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

namespace pliqui
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		public MainForm(String file)
		{
			InitializeComponent();
			
			loadConfig(file);
		}
		private void loadConfig(String file)
		{
			XmlDocument xDoc = new XmlDocument();
			xDoc.Load(file==null?"config.xml":file);
			int wait=0;
			String sWait=xDoc.DocumentElement.GetAttribute("wait");
			if (sWait!=null && (sWait=sWait.Trim()).Length>0) wait=Convert.ToInt32(sWait);

			XmlNodeList apps = xDoc.GetElementsByTagName("app");

			Run.runners=new List<Run>();
			int i=0;
			int w=0;
			int h=0;
			foreach (XmlElement nodo in apps) {
				CheckBox chkList =new CheckBox(); 
				chkList.Text = nodo.GetAttribute("nombre");
				chkList.Left=5;
				chkList.Top=chkList.Height*(i++);
				this.Controls.Add(chkList);
				w=Math.Max(w,chkList.Width);
				h=chkList.Bottom;
				Run r=new Run(nodo,chkList);
				r.Seg=r.Seg+wait;
				Run.runners.Add(r);
			}
			
			this.Width=w+5;
			this.Height=h+25;
			
			if (!Run.starTimer()) this.Text=this.Text+'.';
			Run.form=this;
		}
	}
}
