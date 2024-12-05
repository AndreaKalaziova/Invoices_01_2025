/*  _____ _______         _                      _
 * |_   _|__   __|       | |                    | |
 *   | |    | |_ __   ___| |___      _____  _ __| | __  ___ ____
 *   | |    | | '_ \ / _ \ __\ \ /\ / / _ \| '__| |/ / / __|_  /
 *  _| |_   | | | | |  __/ |_ \ V  V / (_) | |  |   < | (__ / /
 * |_____|  |_|_| |_|\___|\__| \_/\_/ \___/|_|  |_|\_(_)___/___|
 *
 *                      ___ ___ ___
 *                     | . |  _| . |  LICENCE
 *                     |  _|_| |___|
 *                     |_|
 *
 *    REKVALIFIKAČNÍ KURZY  <>  PROGRAMOVÁNÍ  <>  IT KARIÉRA
 *
 * Tento zdrojový kód je součástí profesionálních IT kurzů na
 * WWW.ITNETWORK.CZ
 *
 * Kód spadá pod licenci PRO obsahu a vznikl díky podpoře
 * našich členů. Je určen pouze pro osobní užití a nesmí být šířen.
 * Více informací na http://www.itnetwork.cz/licence
 */

using AutoMapper;
using Invoices.Api.Interfaces;
using Invoices.Api.Models;
using Invoices.Data.Interfaces;
using Invoices.Data.Models;
using Invoices.Data.Repositories;
using System;

namespace Invoices.Api.Managers;

public class PersonManager : IPersonManager
{
    private readonly IPersonRepository personRepository;
    private readonly IMapper mapper;


    public PersonManager(IPersonRepository personRepository, IMapper mapper)
    {
        this.personRepository = personRepository;
        this.mapper = mapper;
    }


    public IList<PersonDto> GetAllPersons()
    {
        IList<Person> persons = personRepository.GetAllByHidden(false);
        return mapper.Map<IList<PersonDto>>(persons);
    }

    public PersonDto? GetPerson(ulong personId)       //pridano pro vypis jedne osoby dle Id
    {
        Person? person = personRepository.FindById(personId);

        if (person is null)
        {
            return null;
        }

        return mapper.Map<PersonDto>(person);
    }

    public PersonDto AddPerson(PersonDto personDto)
    {
        Person person = mapper.Map<Person>(personDto);
        person.PersonId = default;
        Person addedPerson = personRepository.Insert(person);

        return mapper.Map<PersonDto>(addedPerson);
    }

    public void DeletePerson(uint personId)
    {
        HidePerson(personId);
    }

    private Person? HidePerson(ulong personId)
    {
        Person? person = personRepository.FindById(personId);

        if (person is null)
            return null;

        person.Hidden = true;
        return personRepository.Update(person);
    }

    public PersonDto? UpdatePerson(ulong personId, PersonDto updatePersonDto)
    {
        // nalezeni osoby
        Person? person = personRepository.FindById(personId);

        // pokud nenajde, vrati null
        if (person is null)
        {
            return null;
        }

        //nalezene person nastavi hidden na true - tim se schova 
        person.Hidden = true;

        //vytvoreni nove person s updatem a novym Id
        Person updatePerson = mapper.Map<Person>(updatePersonDto);
        updatePerson.PersonId = default;

        //nova osoba pridana do db
        Person addedPerson = personRepository.Insert(updatePerson);

        //vraci novou/upravenou osobu 
        return mapper.Map<PersonDto>(addedPerson);

    }

	public List<StatisticsPersonDto> GetStatisticsPerson()
	{
		IList<Person> persons = personRepository.GetAllByHidden(false);
        List<StatisticsPersonDto> statistic = persons.Select(person => new StatisticsPersonDto
        {
            PersonId = person.PersonId,
            Name = person.Name,
            Revenue = person.InvoicesAsSeller.Sum(invoice => invoice.Price)
        }).ToList();
        return statistic;
    }

}