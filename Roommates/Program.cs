using System;
using System.Collections.Generic;
using Roommates.Repositories;
using Roommates.Models;
using System.Linq;

namespace Roommates
{
    class Program
    {
        //  This is the address of the database.
        //  We define it here as a constant since it will never change.
        private const string CONNECTION_STRING = @"server=localhost\SQLExpress;database=Roommates;integrated security=true;TrustServerCertificate=true;";

        static void Main(string[] args)
        {

            RoomRepository roomRepo = new RoomRepository(CONNECTION_STRING);
            ChoreRepository choreRepo = new ChoreRepository(CONNECTION_STRING);
            RoommateRepository roommateRepo = new RoommateRepository(CONNECTION_STRING);

            //Lightning Exercise
            Roommate roommate1 = new Roommate("Matt", "France", 50, new DateTime(2021, 12, 7), new Room() { });
            Roommate roommate2 = new Roommate()
            {
                FirstName = "Leith",
                LastName = "Abudiab",
                RentPortion = 50,
                MovedInDate = new DateTime(2021, 12, 7),
                Room = new Room() { },
            };

            Console.WriteLine(roommate1.Details);
            Console.WriteLine(roommate2.Details);
            Console.ReadKey();

            bool runProgram = true;
            while (runProgram)
            {
                string selection = GetMenuSelection();

                switch (selection)
                {
                    case ("Show all rooms"):
                        List<Room> rooms = roomRepo.GetAll();
                        foreach (Room r in rooms)
                        {
                            Console.WriteLine($"{r.Name} has an Id of {r.Id} and a max occupancy of {r.MaxOccupancy}");
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Search for room"):
                        Console.Write("Room Id: ");
                        int id = int.Parse(Console.ReadLine());

                        Room room = roomRepo.GetById(id);

                        Console.WriteLine($"{room.Id} - {room.Name} Max Occupancy({room.MaxOccupancy})");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;

                    case ("Add a room"):
                        Console.Write("Room name: ");
                        string name = Console.ReadLine();

                        Console.Write("Max occupancy: ");
                        int max = int.Parse(Console.ReadLine());

                        Room roomToAdd = new Room()
                        {
                            Name = name,
                            MaxOccupancy = max
                        };

                        roomRepo.Insert(roomToAdd);

                        Console.WriteLine($"{roomToAdd.Name} has been added and assigned an Id of {roomToAdd.Id}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;

                    case ("Update a room"):
                        List<Room> roomOptions = roomRepo.GetAll();
                        foreach (Room r in roomOptions)
                        {
                            Console.WriteLine($"{r.Id} - {r.Name} Max Occupancy({r.MaxOccupancy})");
                        }

                        Console.Write("Which room would you like to update? ");
                        int selectedRoomId = int.Parse(Console.ReadLine());
                        Room selectedRoom = roomOptions.FirstOrDefault(r => r.Id == selectedRoomId);

                        Console.Write("New Name: ");
                        selectedRoom.Name = Console.ReadLine();

                        Console.Write("New Max Occupancy: ");
                        selectedRoom.MaxOccupancy = int.Parse(Console.ReadLine());

                        roomRepo.Update(selectedRoom);

                        Console.WriteLine("Room has been successfully updated");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;

                    case ("Delete a room"):
                        List<Room> roomsToDelete = roomRepo.GetAll();
                        foreach (Room r in roomsToDelete)
                        {
                            Console.WriteLine($"{r.Id}-{r.Name}");
                        }

                        Console.WriteLine("Please select a room to delete");
                        int selectRoomId = int.Parse(Console.ReadLine());
                
                        roomRepo.Delete(selectRoomId);

                        List<Room> newRoomList = roomRepo.GetAll();
                        foreach(Room r in newRoomList)
                        {
                            Console.WriteLine($"{r.Id}-{r.Name}");
                        }

                        Console.WriteLine("Room has been deleted");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;

                    case ("Search for Roommate"):
                        Console.Write("Roommate Id: ");
                        int roommateId = int.Parse(Console.ReadLine());

                        Roommate roommate = roommateRepo.GetById(roommateId);

                        Console.WriteLine($"{roommate.FirstName} - {roommate.RentPortion} - {roommate.Room.Name}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;

                    case ("Show all chores"):
                        List<Chore> chores = choreRepo.GetAll();
                        foreach (Chore c in chores)
                        {
                            Console.WriteLine($"{c.Name} has an Id of {c.Id}");
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;

                    case ("Search for chore"):
                        Console.Write("Chore Id: ");
                        int choreId = int.Parse(Console.ReadLine());
                        Chore chore = choreRepo.GetById(choreId);

                        Console.WriteLine($"{chore.Id} - {chore.Name}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;

                    case ("Add a chore"):
                        Console.Write("Chore name:");
                        string choreName = Console.ReadLine();

                        Chore choreToAdd = new Chore()
                        {
                            Name = choreName
                        };

                        choreRepo.Insert(choreToAdd);

                        Console.WriteLine($"{choreToAdd.Name} has been added and assigned an Id of {choreToAdd.Id}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;

                    case "Show unassigned chores":
                        List<Chore> getUnassigned = choreRepo.GetUnassignedChores();

                        foreach (Chore c in getUnassigned)
                        {
                            Console.WriteLine($"{c.Id} - {c.Name} is unassigned");
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadLine();
                        break;

                    case "Assign chore":
                        List<Chore> showUnassigned = choreRepo.GetUnassignedChores();

                        foreach (Chore c in showUnassigned)
                        {
                            Console.WriteLine($"{c.Id} - {c.Name} is unassigned");
                        }
                        Console.Write("Which chore would you like to select? ");
                        int assignChoreId = int.Parse(Console.ReadLine());

                        List<Roommate> showRoommates = roommateRepo.GetAll();

                        foreach (Roommate r in showRoommates)
                        {
                            Console.WriteLine($"{r.Id} - {r.FirstName} {r.LastName}");
                        }

                        Console.Write("Which roommate would you like to have do that chore? ");
                        int assignRoommateId = int.Parse(Console.ReadLine());

                        choreRepo.AssignChore(assignRoommateId, assignChoreId);

                        Console.WriteLine("Chore has been assigned");
                        Console.Write("Press any key to continue.");
                        Console.ReadLine();
                        break;

                    case "Update chore":
                        List<Chore> choreList = choreRepo.GetAll();

                        foreach (Chore r in choreList)
                        {
                            Console.WriteLine($"{r.Name} has an Id of {r.Id}");
                        }

                        Console.Write("Select a chore to update: ");
                        int choreUpdateChoice = int.Parse(Console.ReadLine());

                        Console.WriteLine();
                        Chore selectedChore = choreList.FirstOrDefault(r => r.Id == choreUpdateChoice);

                        Console.Write("Enter a new name: ");
                        selectedChore.Name = Console.ReadLine();

                        Console.WriteLine();

                        choreRepo.Update(selectedChore);

                        Console.WriteLine("Room has been updated");
                        Console.WriteLine("Press any key to continue");
                        Console.ReadLine();
                        break;

                    case "Delete chore":
                        List<Chore> choresToDelete = choreRepo.GetAll();

                        foreach (Chore c in choresToDelete)
                        {
                            Console.WriteLine($"{c.Id} - {c.Name}");
                        }

                        Console.Write("Which chore do you want to delete? ");
                        int choreDelId = int.Parse(Console.ReadLine());

                        choreRepo.Delete(choreDelId);

                 
                        Console.WriteLine("Chore has been deleted");                 
                        Console.WriteLine("Press any key to continue");
                        Console.ReadLine();
                        break;

                    case ("Exit"):
                        runProgram = false;
                        break;
                }
            }
        }

        static string GetMenuSelection()
        {
            Console.Clear();

            List<string> options = new List<string>()
            {
                "Show all rooms",
                "Search for room",
                "Add a room",
                "Update a room",
                "Delete a room",
                "Search for Roommate",
                "Show all chores",
                "Search for chore",
                "Add a chore",
                "Show unassigned chores",
                "Assign chore",
                "Update chore",
                "Delete chore",
                "Exit"
            };

            for (int i = 0; i < options.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {options[i]}");
            }

            while (true)
            {
                try
                {
                    Console.WriteLine();
                    Console.Write("Select an option > ");

                    string input = Console.ReadLine();
                    int index = int.Parse(input) - 1;
                    return options[index];
                }
                catch (Exception)
                {

                    continue;
                }
            }
        }
    }
}
