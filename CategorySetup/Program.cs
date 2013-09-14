using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mappen.Domain.Entities.Events;
using Mappen.Data.IocContainersData;
using Cibus.Infrastructure.IocBox;
using Mappen.Data.Events;
using Ninject;

namespace CategorySetup
{
	class Program
	{
		static void Main(string[] args)
		{
			Box.Module = new StandardKernel(new RealDataModule());

			EventCategory tribes = new EventCategory("Tribes"),
					gender = new EventCategory("Gender", tribes),
					ethnicity = new EventCategory("Ethnicity", tribes),
					politics = new EventCategory("Politics", tribes),
					doingGood = new EventCategory("Doing Good", tribes),
					dancing = new EventCategory("Dancing", tribes),
					spirituality = new EventCategory("Spirituality", tribes),
					relationships = new EventCategory("Relationships", tribes),
					business = new EventCategory("Business", tribes),
					solidarity = new EventCategory("Solidarity", tribes),
					family = new EventCategory("Family", tribes),
				humanities = new EventCategory("Being Human"),
				art = new EventCategory("Art", humanities),
				music = new EventCategory("Music", humanities),
					rock = new EventCategory("Rock", music),
					classical = new EventCategory("Classical", music),
					jazz = new EventCategory("Jazz", music),
					world = new EventCategory("World", music),
					pop = new EventCategory("Pop", music),
					electronic = new EventCategory("Electronic", music),
				literature = new EventCategory("Literature", humanities),
				dance = new EventCategory("Dance", humanities),
				fashion = new EventCategory("Fashion", humanities),
				fitness = new EventCategory("Fitness", humanities),
				film = new EventCategory("Film", humanities),
				gastronomy = new EventCategory("Gastronomy", humanities),
				autoTech = new EventCategory("Auto and Tech", humanities),
				education = new EventCategory("Education", humanities),
				games = new EventCategory("Games", humanities),
				hobbies = new EventCategory("Hobbies", humanities),
				animals = new EventCategory("Animals", humanities),
				recreation = new EventCategory("Recreation", humanities),
				gettingOut = new EventCategory("Getting Out", humanities);

			using (EventUnitOfWork uow = new EventUnitOfWork())
			{
				uow.EventCategories.AddRange(new List<EventCategory>() { 
					tribes,
					gender,
					ethnicity,
					politics,
					doingGood,
					dancing,
					spirituality,
					relationships,
					business,
					solidarity,
					family,
					humanities,
					art,
					music,
					rock,
					classical,
					jazz,
					electronic,
					pop,
					world,
					literature,
					dance,
					fashion,
					fitness,
					film,
					gastronomy,
					autoTech,
					education,
					games,
					hobbies,
					animals,
					recreation,
					gettingOut
					
				});
				uow.Save();
			}
		}
	}
}
