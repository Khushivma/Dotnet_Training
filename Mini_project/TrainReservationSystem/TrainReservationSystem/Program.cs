using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace TrainReservationSystem
{
    class Program
    {
        static TrainSystemEntities1 db = new TrainSystemEntities1();
        static void Main(string[] args)
        {
                Console.WriteLine("***********************Welcome to the Indian Railway Passenger Reservation Enquiry! ********************************");
                Console.WriteLine("------------------------------------------------------------------------------------------------------------------->");
                Console.WriteLine("1. Admin -> Press 1");
                Console.WriteLine("2. User -> Press 2");
                Console.WriteLine("3. Exit -> Press 3");
                Console.WriteLine("------------------------------------------------------------------------------------------------------------------->");
                Console.Write("YOUR CHOICE: ");
                string choice = Console.ReadLine();
                Console.Clear();

                Console.WriteLine("------------------------------------------------------------------------------------------------------------------->");
                
            switch (choice)
                {
                    case "1":
                        Admin();
                        break;
                    case "2":
                        UserMenu();
                        break;
                    case "3":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
            static void UserMenu()
            {
                Console.WriteLine("Enter Your User Details:");
                Console.WriteLine("1. Existing User -> Press 1");
                Console.WriteLine("2. New User -> Press 2");
                Console.WriteLine("------------------------------------------------------------------------------------------------------------------>");
                Console.Write("YOUR CHOICE: ");
                string userChoice = Console.ReadLine();
                Console.WriteLine("------------------------------------------------------------------------------------------------------------------>");

            switch (userChoice)
                {
                    case "1":
                        ExistingUserLogin();
                        break;
                    case "2":
                        RegisterNewUser();
                        break;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }

            static void ExistingUserLogin()
            {
                Console.WriteLine("Enter your username:");
                string username = Console.ReadLine();

                Console.WriteLine("Enter your password:");
                string password = Console.ReadLine();
                Console.Clear();
                User_option();
            }

            static void RegisterNewUser()
            {
                Console.WriteLine("Enter your username:");
                string username = Console.ReadLine();

                Console.WriteLine("Enter your password:");
                string password = Console.ReadLine();


                var newUser = new User { UserName = username, Password = password };
                db.Users.Add(newUser);
                Console.Clear();
                db.SaveChanges();


                Console.WriteLine("User registered successfully!");
            }


            static void User_option()
            {
                bool exist = false;
                while (!exist)
                {
                    Console.WriteLine("---------------------------------------------------------------------------------------------------------------------------");
                    Console.WriteLine("1. View all trains");
                    Console.WriteLine("2. Book a ticket");
                    Console.WriteLine("3. Cancel a booking");
                    Console.WriteLine("4. Admin functions");
                    Console.WriteLine("5. Exit");
                    Console.Write("Choose an option: ");
                    string choice = Console.ReadLine();
                    Console.WriteLine("-----------------------------------------------------------------------------------------------------------------------------");

                    switch (choice)
                    {
                        case "1":
                            ViewAllTrains();
                            break;
                        case "2":
                            BookTicket();
                            break;
                        case "3":
                            CancelBooking();
                            break;
                        case "4":
                            AdminMenu();
                            break;

                            
                        case "5":
                            exist = true;
                            break;
                        default:
                            Console.WriteLine("Invalid option. Please try again.");
                            break;
                    }
                }
            }


            static void ViewAllTrains()
            {
                 var trains = db.Trains.ToList();

            // Print header
                 Console.WriteLine("--------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                 Console.WriteLine("| Train ID  |       Name                       |          From            |          To          |       Class     |    Total Seats |    Available Seats |   Price   |");
                 Console.WriteLine("---------------------------------------------------------------------------------------------------------------------------------------------------------------------------");

            // Print train details
            foreach (var train in trains)
            {
                Console.WriteLine($"| {train.TrainNo,-9} |   {train.TrainName,-16}             |  {train.From,-17}        |  {train.To,-15}       |  {train.Class,-9}  |  {train.TotalSeats,-11}   |   {train.AvailableSeats,-15}   |  {train.Price,-9}  |");
            }
        }
        static void BookTicket()
        {
            try
            {
                Console.WriteLine("Available Trains:");
                ViewAllTrains();
                Console.Write("Enter your name: ");
                string customerName = Console.ReadLine();

                Console.Write("Enter the Train No of the train you want to book: ");
                int trainNo = int.Parse(Console.ReadLine());

                Console.Write("Enter the class : ");
                string ticketClass = Console.ReadLine();

                Console.Write("Enter the date of travel : ");
                DateTime dateOfTravel;
                if (!DateTime.TryParse(Console.ReadLine(), out dateOfTravel))
                {
                    Console.WriteLine("Invalid date format. Please enter date in yyyy-MM-dd format.");
                    return;
                }

                Console.Write("Enter the total number of seats: ");
                int totalSeats;
                if (!int.TryParse(Console.ReadLine(), out totalSeats) || totalSeats <= 0)
                {
                    Console.WriteLine("Invalid number of seats. Please enter a valid positive integer.");
                    return;
                }

                var TotalamountParam = new SqlParameter("@Totalamount", SqlDbType.Float);
                TotalamountParam.Direction = ParameterDirection.Output;

                var PassengerNameParam = new SqlParameter("@PassengerName", customerName);
                var trainNoParam = new SqlParameter("@TrainNo", trainNo);
                var classParam = new SqlParameter("@Class", ticketClass);
                var dateOfTravelParam = new SqlParameter("@DateOfTravel", dateOfTravel);
                var totalSeatsParam = new SqlParameter("@TotalSeats", totalSeats);

                int rowsAffected = db.Database.ExecuteSqlCommand(
                    "EXEC InsertBookingAndUpdateTrainWithDates @PassengerName, @TrainNo, @Class, @DateOfTravel, @TotalSeats, @Totalamount",
                    PassengerNameParam, trainNoParam, classParam, dateOfTravelParam, totalSeatsParam, TotalamountParam);

                if (rowsAffected > 0)
                {
                    var newBookingId = db.bookings
                        .Where(b => b.PassengerName == customerName && b.TrainNo == trainNo && b.Class == ticketClass && b.Date_of_Travel == dateOfTravel)
                        .Select(b => b.Booking_ID)
                        .FirstOrDefault();

                    if (newBookingId != 0)
                    {

                        Console.WriteLine($"Ticket booked successfully! Your booking ID is: {newBookingId}");
                        Console.WriteLine($"Passenger Name: {customerName}, TrainNo: {trainNo}, Class: {ticketClass}, Date_of_Travel: {dateOfTravel}, Totalamount: {TotalamountParam.Value}, Number_of_Seats: {totalSeats}");
                    }
                }
                else
                {
                    Console.WriteLine("Booking failed. Please try again.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        static void ShowBookedTickets()
        {
            Console.WriteLine("enter your booking id");
            int bookingId = Convert.ToInt32(Console.ReadLine());
            var bookedTickets = db.bookings.Where(bt => bt.Booking_ID == bookingId).ToList();


            if (bookedTickets.Count == 0)
            {
                Console.WriteLine("No booked tickets found for the specified user.");
                return;
            }

            Console.WriteLine("Booked Tickets:");
            Console.WriteLine("-----------------------------------------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine(" Train No  | Train Name       |  customer name |   date of travel    | Class  |   Booking Date |");
            Console.WriteLine("------------------------------------------------------------------------------------------------------------------------------------------------------");

            foreach (var ticket in bookedTickets)
            {
                var train = db.Trains.FirstOrDefault(sa => sa.TrainNo == ticket.TrainNo);

                Console.WriteLine($"|{ticket.TrainNo,-9} | {train.TrainName,-15} | {ticket.Class,-6}  {ticket.Date_of_Travel,-21} |");
            }

            Console.WriteLine("-----------------------------------------------------------------------------------------------------------------------------------------------------");
        }


        static void CancelBooking()
            {
                Console.WriteLine("---------------------------------------------------------------------------");
                try
                {


                    Console.WriteLine("enter your booking id");
                    int bookingId = Convert.ToInt32(Console.ReadLine());

                    var booking = db.bookings.FirstOrDefault(b => b.Booking_ID == bookingId);

                    if (booking == null)
                    {

                        Console.WriteLine("No booked ticket for cancel, Please book the tickets");

                        return;
                    }
                    else
                    {
                        Console.WriteLine("Booking details:");
                        Console.WriteLine($"Customer Name: {booking.PassengerName}");
                        Console.WriteLine($"Booking ID: {booking.Booking_ID}");
                        Console.WriteLine($"Number of Tickets: {booking.Totalseats}");
                        Console.WriteLine($"Date of Travel: {booking.Date_of_Travel}");
                        Console.WriteLine($"Class:{booking.Class}");
                        //double refundamount = (double)0.0m;
                        decimal refundamount = (decimal)booking.Totalamount * 0.75m; // Assuming 75% refund policy
                        Console.WriteLine($"Refund amount: {refundamount}");
                        ////double refund = (double)booking.total_amount * 0.75;
                        //Console.WriteLine($"Refund Amount:{refundamount}");

                        // Add cancelled ticket to the database
                        var cancelledTicket = new cancellation
                        {
                            Booking_ID = bookingId, // Assuming canceledId is PNR
                            PassengerName = booking.PassengerName,
                            TrainNo = booking.TrainNo,
                            Cancel_Date = DateTime.Now,
                            Refund = (int?)refundamount
                        };

                        db.cancellations.Add(cancelledTicket);
                        // Retrieve the train entity including the available_seats property


                        db.SaveChanges();

                        // Remove booked ticket from the database
                        db.bookings.Remove(booking);


                        Console.WriteLine("Refund amount will be initated soon!");
                        Console.WriteLine("Cancellation successful!");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error cancelling booking: {ex.Message}");
                }
                Console.WriteLine("---------------------------------------------------------------------------");

            }


            static void Admin()
            {
                Console.WriteLine("---------------------------------------------------------------------------");
                Console.Write("Enter username: ");
                string username = Console.ReadLine();

                Console.Write("Enter password: ");
                string password = Console.ReadLine();
                var admin = db.AdminDetails.FirstOrDefault(a => a.AdminName == username && a.Password == password);

                if (admin == null)
                {
                    Console.WriteLine("Invalid username or password.");

                }
                else
                {

                    Console.WriteLine("Admin login successful.");
                }
                Console.WriteLine("---------------------------------------------------------------------------");
                AdminMenu();

            }

            private static void AdminMenu()
            {
                while (true)
                {
                    Console.WriteLine("---------------------------------------------------------------------------");
                    Console.WriteLine("Admin Menu:");
                    Console.WriteLine("1. Add Train");
                    Console.WriteLine("2. Modify Train");
                    Console.WriteLine("3. Delete Train");
                    Console.WriteLine("4. Exit");

                    Console.Write("Enter your choice: ");
                    int choice;
                    if (!int.TryParse(Console.ReadLine(), out choice))
                    {
                        Console.WriteLine("Invalid input. Please enter a number.");
                        continue;
                    }

                    switch (choice)
                    {
                        case 1:
                            AddTrain();
                            break;
                        case 2:
                            ModifyTrain();
                            break;
                        case 3:
                            DeleteTrain();
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please select a valid option.");

                            break;
                    }
                    Console.WriteLine("---------------------------------------------------------------------------");
                }
            }

            private static void AddTrain()
            {

                Console.WriteLine("---------------------------------------------------------------------------");
                Console.WriteLine("Enter train details:");

                Console.Write("Train No: ");
                int trainNo = Convert.ToInt32(Console.ReadLine());

                Console.Write("Train Name: ");
                string trainName = Console.ReadLine();

                Console.Write("Departure Station: ");
                string departureStation = Console.ReadLine();

                Console.Write("Arrival Station: ");
                string arrivalStation = Console.ReadLine();

                Console.Write("Departure Time: ");
                TimeSpan departureTime;
                if (!TimeSpan.TryParse(Console.ReadLine(), out departureTime))
                {
                    Console.WriteLine("Invalid departure time format.");

                }

                Console.Write("Arrival Time: ");
                TimeSpan arrivalTime;
                if (!TimeSpan.TryParse(Console.ReadLine(), out arrivalTime))
                {
                    Console.WriteLine("Invalid arrival time format.");

                }

                // Assuming the train is active by default
                string status = "active";

                // Create a new train object
                var train = new Train
                {
                    TrainNo = trainNo,
                    TrainName = trainName,
                    From = departureStation,
                    To = arrivalStation,
                    Departure_Time = departureTime,
                    Arrival_Time = arrivalTime,
                    Status = status
                };

                // Add the train to the trains table
                db.Trains.Add(train);

                try
                {
                    // Save changes to the database
                    db.SaveChanges();
                    Console.WriteLine("Train added successfully!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error adding train: {ex.Message}");
                }
                Console.WriteLine("---------------------------------------------------------------------------");
            }
            private static void ModifyTrain()
            {
                Console.WriteLine("---------------------------------------------------------------------------");
                Console.Write("Enter the Train No of the train you want to modify: ");
                int trainNo = Convert.ToInt32(Console.ReadLine());

                // Find the train with the provided train number
                var train = db.Trains.FirstOrDefault(t => t.TrainNo == trainNo);
                if (train == null)
                {
                    Console.WriteLine("Train not found.");

                }

                // Display current details of the train
                Console.WriteLine("Current details of the train:");
                Console.WriteLine($"Train No: {train.TrainNo}");
                Console.WriteLine($"Train Name: {train.TrainName}");
                Console.WriteLine($"Source: {train.To}");
                Console.WriteLine($"Destination: {train.From}");
                Console.WriteLine($"Departure Time: {train.Departure_Time}");
                Console.WriteLine($"Arrival Time: {train.Arrival_Time}");
                Console.WriteLine($"status: {(train.Status == "yes" ? "active" : "inactive")}");
                // Prompt user to enter new details
                Console.WriteLine("Enter new details:");

                Console.Write("Train Name: ");
                string newTrainName = Console.ReadLine();
                if (!string.IsNullOrEmpty(newTrainName))
                {
                    train.TrainName = newTrainName;
                }

                Console.Write("Source: ");
                string newSource = Console.ReadLine();
                if (!string.IsNullOrEmpty(newSource))
                {
                    train.To = newSource;
                }

                Console.Write("Destination: ");
                string newDestination = Console.ReadLine();
                if (!string.IsNullOrEmpty(newDestination))
                {
                    train.From = newDestination;
                }
                db.SaveChanges();

                Console.WriteLine("Train modified successfully!");
                Console.WriteLine("---------------------------------------------------------------------------");
            }

            public static void DeleteTrain()
            {
                Console.WriteLine("---------------------------------------------------------------------------");
                Console.Write("Enter Train No of the train you want to deactivate: ");
                int trainNo = Convert.ToInt32(Console.ReadLine());

                // Find the train in the database
                var train = db.Trains.FirstOrDefault(t => t.TrainNo == trainNo);
                if (train == null)
                {
                    Console.WriteLine("Train not found.");

                }
                else
                {
                    Console.WriteLine("Do you want to deactive the train (y/n)");
                    train.Status = "deactive";
                }

                try
                {
                    // Save changes to the database
                    db.SaveChanges();
                    Console.WriteLine("Train deactivated successfully!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error deactivating train: {ex.Message}");
                }
                Console.WriteLine("---------------------------------------------------------------------------");
            }
    }
}

