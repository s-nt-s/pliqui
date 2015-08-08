/*
 * Creado por SharpDevelop.
 * Usuario: 99GU5372
 * Fecha: 28/11/2013
 * Hora: 12:01
 * 
 * Para cambiar esta plantilla use Herramientas | Opciones | Codificación | Editar Encabezados Estándar
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using System.Xml;

namespace pliqui
{
	/// <summary>
	/// Description of run.
	/// </summary>
	public class Run
	{
		public static List<Run> runners;
		private static Timer timer;
		private static int clock=0;
		private static int count=1;
		private static Run current=null;
		public static MainForm form;
		private String nombre;
		private CheckBox chk;
		private String prc=null;
		Boolean iu;
		
		public string Nombre {
			get { return nombre; }
		}
		private String exe;
		
		public string Exe {
			get { return exe; }
		}
		private int seg;
		
		public int Seg {
			get { return seg; }
			set { seg=value; }
		}
		private String arg;
		
		public string Arg {
			get { return arg; }
		}
		
		public Run(XmlElement nodo,CheckBox chkList)
		{
			chk=chkList;
			iu="true".Equals(trim(nodo.GetAttribute("iu")));
			chk.Checked=!iu;
			nombre=trim(nodo.GetAttribute("nombre"));
			exe=trim(nodo.GetAttribute("exe"));
			arg=trim(nodo.GetAttribute("arg"));
			if (!"true".Equals(trim(nodo.GetAttribute("dup")))) {
				prc=trim(nodo.GetAttribute("prc"));
				if (prc==null) {
					String[] spt=exe.Split('\\');
					spt=spt[spt.Length-1].Split('.');
					prc=trim(spt[0]);
				}
				this.isRunning();
			}
			String min=trim(nodo.GetAttribute("min"));
			String s=trim(nodo.GetAttribute("seg"));
			seg=0;
			if (min!=null) seg=seg+(Convert.ToInt32(min)*60);
			if (s!=null) seg=seg+(Convert.ToInt32(s));
			chk.CheckedChanged+=new EventHandler(this.check);
		}
		
		private void check(Object sender, EventArgs e) {
			if (!this.chk.Checked || this.isRunning()) return;
			if (Run.timer.Enabled && Run.clock<this.Seg) return;
			this.run();
		}

		public static String trim(String s) {
			return (s==null || (s=s.Trim()).Length==0)?null:s;
		}

		public Boolean isRunning() {
			if (prc==null) return false;
			Process[] prs=System.Diagnostics.Process.GetProcessesByName(prc);
			if(prs==null || prs.Length==0) return false;
			this.chk.Checked=true;
			this.chk.Enabled=false;
			return true;
		}
		
		public Boolean needRun() {
			return chk.Checked && chk.Enabled && !isRunning();
		}
		
		public static void run(object sender, EventArgs e) {
			if (Run.current.needRun()) Run.current.run();
			int s=getSiguiente();
			if (s>0) Run.timer.Interval=s;
			else {
				Run.timer.Stop();
				form.Text=form.Text+'.';
			}
		}
		
		public void run () {
			System.Diagnostics.Process.Start(@exe,arg);
			chk.Checked=true;
			chk.Enabled=false;
			chk.Text=nombre;
		}
		
		public static Boolean starTimer() {
			if (Run.timer==null) Run.timer = new Timer();
			int s=getSiguiente();
			if (s<0) return false;
			Run.timer.Interval=s;
			Run.timer.Tick += Run.run;
			Run.timer.Start();
			return true;
		}
		
		public static int getSiguiente() {
			int min=-1;
			foreach(Run r in Run.runners) {
				if (r.needRun() && r.Seg>clock) {
					if (min==-1 || r.Seg<min) {
						min=r.Seg;
						Run.current=r;
					}
				}
			}
			int itr=min-clock;
			if (itr<0 || Run.current==null) return -1;
			Run.current.chk.Text=(Run.current.nombre+" <- "+(Run.count++));
			clock=min;
			return itr*1000;
		}
	}
}
