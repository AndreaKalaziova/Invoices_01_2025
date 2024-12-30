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

namespace Invoices.Api.Managers;

public class PersonManager : IPersonManager
{
    private readonly IPersonRepository personRepository;    // repository to manage db operations for Person entities
    private readonly IMapper mapper;                        // automapper for mapping between entity and DTO objects


    public PersonManager(IPersonRepository personRepository, IMapper mapper)
    {
        this.personRepository = personRepository;   //DI for Person repository
        this.mapper = mapper;                       //DI for AutoMapper
    }

    /// <summary>
    /// get all Persons that are active (not hidden)
    /// </summary>
    /// <returns>List of non-hodden Persons mapped to PersonDto objects</returns>
    public IList<PersonDto> GetAllPersons()
    {
        //get all Persons where 'hidden' is false (active person)
        IList<Person> persons = personRepository.GetAllByHidden(false);
        //map the Person entities to PersonDto objects and return the list of (active) Persons
        return mapper.Map<IList<PersonDto>>(persons);
    }

    /// <summary>
    /// get a person by its Id
    /// </summary>
    /// <param name="personId"></param>
    /// <returns>PersonDto object of the found Person, or null if not found</returns>
    public PersonDto? GetPerson(ulong personId)       
    {
        //find the Person by Id
        Person? person = personRepository.FindById(personId);

        //if the person does not exist, return null
        if (person is null)
            return null;

        //map the Person entuty to PersonDto object and return it
        return mapper.Map<PersonDto>(person);
    }

    /// <summary>
    /// add new Person to db
    /// </summary>
    /// <param name="personDto"></param>
    /// <returns>added Person as PersonDto object</returns>
    public PersonDto AddPerson(PersonDto personDto)
    {

		// Check if a person with the same IdentificationNumber already exists
		if (personRepository.ExistsWithIdentificationNumber(personDto.IdentificationNumber))
			throw new InvalidOperationException($"IČO {personDto.IdentificationNumber} je již použito.");
		
		Person person = mapper.Map<Person>(personDto);          //map the PersonDto to a Person entity
        person.PersonId = default;                              //new Id added to new Person
        Person addedPerson = personRepository.Insert(person);   // insert the new added Person into repository and etrieve it

		//map the added Person entity back to PersonDto and return ii
		return mapper.Map<PersonDto>(addedPerson);
    }

    /// <summary>
    /// person is not deleted from db, only hidden
    /// </summary>
    /// <param name="personId"></param>
    public void DeletePerson(uint personId)
    {
        //hide the Person (bool hidden = true)
        HidePerson(personId);
    }

    /// <summary>
    /// marks the Person as hidden in db
    /// </summary>
    /// <param name="personId"></param>
    /// <returns>the hidden Person or null, if not found</returns>
    private Person? HidePerson(ulong personId)
    {
		//find the Person by Id
		Person? person = personRepository.FindById(personId);

        //if the person does not exist, return null
        if (person is null)
            return null;

        //bool hidden change to true -> inactive
        person.Hidden = true;
        //update the person in repository and return the updated entity
        return personRepository.Update(person);
    }
    /// <summary>
    /// update Person with new data, generating new Id, original person with origin Id remains hidden
    /// </summary>
    /// <param name="personId"></param>
    /// <param name="updatePersonDto"></param>
    /// <returns>updated Person with new Id</returns>
    public PersonDto? UpdatePerson(ulong personId, PersonDto updatePersonDto)
    {
        // get Person by Id
        Person? person = personRepository.FindById(personId);

        // if Person not found, return null
        if (person is null)
            return null;

        // Check for attempts to modify immutable fields IdentificationNumber
        if (person.IdentificationNumber != updatePersonDto.IdentificationNumber)
		{
			throw new InvalidOperationException("IČO nelze měnit");
		}

		//if Person found, bool hidden changes to true -> hence is hidden and not active
		person.Hidden = true;

        //create new Person entity with updated details and new Id
        Person updatePerson = mapper.Map<Person>(updatePersonDto);
        updatePerson.PersonId = default;

        // new/updated Person added to db
        Person addedPerson = personRepository.Insert(updatePerson);

        //return new/updated Person entity mapped to PersonDto
        return mapper.Map<PersonDto>(addedPerson);
    }

    /// <summary>
    /// get statistics for all Persons
    /// </summary>
    /// <returns>PersonId - its name - its revenue - its turnover - its profit</returns>
	public List<StatisticsPersonDto> GetStatisticsPerson()
	{
        // get all active (not-hidden) Person from repository
		IList<Person> persons = personRepository.GetAllByHidden(false);
        int previousYear = DateTime.Now.Year - 1;               //previus year for filtaring

        //calculate statistics for each person and map them to StisticsPersonDto objects
        List<StatisticsPersonDto> statistic = persons.Select(person => new StatisticsPersonDto
        {
            PersonId = person.PersonId,                                         //Person's unique Id
            Name = person.Name,                                                 //Person's Name
            Revenue = person.InvoicesAsSeller.Sum(invoice => invoice.Price),    //total revenue from sales
            PreviousYearTurnover = person.InvoicesAsSeller                          //turnover from last year
                .Where(invoice => invoice.Issued.Year == previousYear)
                .Sum(invoice => invoice.Price) +
				person.InvoicesAsBuyer
			    .Where(invoice => invoice.Issued.Year == previousYear)
			    .Sum(invoice => invoice.Price),
            Profit = person.InvoicesAsSeller.Sum(invoice => invoice.Price) -    //profit 
                    person.InvoicesAsBuyer.Sum(invoice => invoice.Price),
		}).ToList();
        //retunr the List of the statistics
        return statistic;
    }
}