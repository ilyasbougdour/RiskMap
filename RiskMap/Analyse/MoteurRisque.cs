using System;
using System.Collections.Generic;

namespace RiskMap
{
	/// <summary>
	/// MODULE ANALYSE (Abdelali) - calcul du score et du niveau de chaque machine.
	/// Version de départ fonctionnelle : règles par port + points selon la CVE la plus grave de chaque service.
	/// Choix fait : on ne compte que la CVE la plus grave par service (sinon un vieil Apache
	/// avec 40 CVE dépasserait tout le reste). À valider ou changer en équipe.
	/// </summary>
	public class MoteurRisque
	{
		private GrilleRisque grille;

		public MoteurRisque(GrilleRisque prmGrille)
		{
			grille = prmGrille;
		}

		//évalue toutes les machines du résultat
		public void evaluer(ResultatScan r){
			foreach(Machine m in r.Machines)
				this.evaluer(m);
		}

		//remplit Score, Niveau et DetailScore d'une machine
		public void evaluer(Machine m){
			int score = 0;
			m.DetailScore.Clear();

			//1. règles par port
			foreach(RegleRisque regle in grille.Regles){
				if (m.portOuvert(regle.Port) && regle.Points > 0){
					score = score + regle.Points;
					m.DetailScore.Add("+" + regle.Points.ToString() + " port " + regle.Port.ToString() + " : " + regle.Raison);
				}
			}

			//2. CVE : la plus grave de chaque service
			foreach(Service s in m.Services){
				double max = s.scoreMax();
				int points = this.pointsCve(max);
				if (points > 0){
					score = score + points;
					m.DetailScore.Add("+" + points.ToString() + " " + s.description() + " : CVE de score " + max.ToString());
				}
				if (s.EtatVersion == EtatInfo.Masque)
					m.DetailScore.Add("? " + s.description() + " : version masquée, risque inconnu (pas faible)");
			}

			m.Score = score;
			m.Niveau = this.niveau(score);
		}

		//points pour un score CVSS
		public int pointsCve(double cvss){
			if (cvss >= 9.0)
				return grille.PointsCveCritique;
			else if (cvss >= 7.0)
				return grille.PointsCveElevee;
			else if (cvss >= 4.0)
				return grille.PointsCveMoyenne;
			else
				return 0;
		}

		//niveau pour un score
		public NiveauRisque niveau(int score){
			if (score >= grille.SeuilCritique)
				return NiveauRisque.Critique;
			else if (score >= grille.SeuilEleve)
				return NiveauRisque.Eleve;
			else if (score >= grille.SeuilMoyen)
				return NiveauRisque.Moyen;
			else
				return NiveauRisque.Faible;
		}
	}
}
