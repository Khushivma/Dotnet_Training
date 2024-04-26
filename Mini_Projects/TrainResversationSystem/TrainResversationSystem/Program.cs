using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace TrainResversationSystem
{
    class Program
    {
        static TrainRerversationSystemEntities01 db = new TrainRerversationSystemEntities01();
        static void Main(string[] args)
        {
            int lineLength = Console.WindowWidth - "Welcome to the Indian Railway Passenger Reservation Enquiry!".Length;
            int padding = lineLength / 2;
            string paddingStr = new string(' ', padding);
            Console.WriteLine(paddingStr + "******************************************************************************************************************");
            Console.WriteLine(paddingStr + "|                                                                                                                |");
            Console.WriteLine(paddingStr + "*                             Welcome to the Indian Railway Passenger Reservation Enquiry!                       *");
            Console.WriteLine(paddingStr + "|                                                                                                                |");
            Console.WriteLine(paddingStr + "******************************************************************************************************************");
            Console.WriteLine("                                                                                                                             ");
            Console.WriteLine("1. Admin -> Press 1");
            Console.WriteLine("2. User -> Press 2");
            Console.WriteLine("3. Exit -> Press 3");
            Console.WriteLine("------------------------------------------------------------------------------------------------------------------------------------------------>");
            Console.Write("YOUR CHOICE: ");
            Console.WriteLine("                                                   ");
            string choice = Console.ReadLine();

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
            Console.WriteLine("                                                   ");
            Console.WriteLine("Enter Your User Details:");
            Console.WriteLine("1. Existing User -> Press 1 ");
            Console.WriteLine("2. New User -> Press 2");
            Console.WriteLine("------------------------------------------------------------------------------------------------------------------------------------------------>");
            Console.Write("YOUR CHOICE: ");
            Console.WriteLine("                                                   ");

            string userChoice = Console.ReadLine();

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
            Console.WriteLine("                                                   ");
            Console.WriteLine("Enter your username:");
            string username = Console.ReadLine();

            Console.WriteLine("Enter your password:");
            string password = Console.ReadLine();
            Console.Clear();
            User_option();
            
        }

        static void RegisterNewUser()
        {
            Console.WriteLine("                                                   ");
            Console.WriteLine("Enter your username:");
            string username = Console.ReadLine();

            Console.WriteLine("Enter your password:");
            string password = Console.ReadLine();


            var newUser = new User { UserName = username, Password = password };
            db.Users.Add(newUser);
            Console.Clear();
            db.SaveChanges();


            Console.WriteLine("Successful registration completed!");
        }


        static void User_option()
        {
            int lineLength = Console.WindowWidth - "Welcome to the main menu".Length;
            int padding = lineLength / 3;
            string paddingStr = new string(' ', padding);
            Console.WriteLine(paddingStr + "***************************************************************************");
            Console.WriteLine(paddingStr + "|                                                                         |");
            Console.WriteLine(paddingStr + "|                    Welcome to the main menu                             |");
            Console.WriteLine(paddingStr + "|                                                                         |");
            Console.WriteLine(paddingStr + "***************************************************************************");
            bool exist = false;
            while (!exist)
            {
                Console.WriteLine("------------------------------------------------------------------------------------------------------------------------------------------------------------------>");
                Console.WriteLine("                                                   ");
                Console.WriteLine("1. View all trains");
                Console.WriteLine("2. Book a ticket");
                Console.WriteLine("3. Cancel a booking");
                Console.WriteLine("4. Show all booking tickets");
                Console.WriteLine("5. Show all cancelled tickets");
                Console.WriteLine("6. Admin functions");
                Console.WriteLine("7. Exit");
                Console.Write("Choose an option: ");
                string choice = Console.ReadLine();
                Console.WriteLine("------------------------------------------------------------------------------------------------------------------------------------------------------------------->");

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
                        ShowBookedTickets();
                        break;
                    case "5":
                        ShowCancelTickets();
                        break;
                    case "6":
                        AdminMenu();
                        break;
                    case "7":
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
            Console.WriteLine("---------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("| Train ID  |       Name                |          From           |          To          |       Class     |    Total Seats |    Available Seats |   Price   |    Status  |");
            Console.WriteLine("---------------------------------------------------------------------------------------------------------------------------------------------------------------------------");

            // Print train details
            foreach (var train in trains)
            {
                Console.WriteLine($"| {train.TrainNo,-9} | {train.TrainName,-25} | {train.From,-23} | {train.To,-20} | {train.Class,-15} | {train.TotalSeats,-14} | {train.AvailableSeats,-16} | {train.Price,-9} | {train.Status,-15}| ");
            }

            Console.WriteLine("--------------------------------------------------------------------------------------------------------------------------------------------------------------------->");

        }

        static void BookTicket()
        {
            try
            {
                Console.WriteLine("                                                   ");
                Console.WriteLine("Available Trains:");
                ViewAllTrains();
                Console.WriteLine("                                                   ");
                Console.Write("Enter Passenger Name: ");
                string customerName = Console.ReadLine();

                Console.Write("Enter the Train number: ");
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
                var todayDate = DateTime.Today;
                if (dateOfTravel <= todayDate)
                {
                    Console.WriteLine("Date of travel should be greater");
                    return;
                }
                Console.Write("Enter the total number of seats: ");
                int totalSeats;
                if (!int.TryParse(Console.ReadLine(), out totalSeats) || totalSeats <= 0)
                {
                    Console.WriteLine("Invalid number of seats. Please enter a valid positive integer.");
                    return;
                }

                // Check if the train is active
                var trainStatus = db.Trains.Where(t => t.TrainNo == trainNo).Select(t => t.Status).FirstOrDefault();
                if (!string.Equals(trainStatus, "Active", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Booking failed. Please try when the train is in Active status.");
                    return;
                }

                // Output parameter for Totalamount
                var TotalamountParam = new SqlParameter("@Totalamount", SqlDbType.Float);
                TotalamountParam.Direction = ParameterDirection.Output;

                // Other input parameters
                var PassengerNameParam = new SqlParameter("@PassengerName", customerName);
                var trainNoParam = new SqlParameter("@TrainNo", trainNo);
                var classParam = new SqlParameter("@Class", ticketClass);
                var dateOfTravelParam = new SqlParameter("@DateOfTravel", dateOfTravel);
                var totalSeatsParam = new SqlParameter("@TotalSeats", totalSeats);

                // Execute the stored procedure
                int rowsAffected = db.Database.ExecuteSqlCommand(
                    "EXEC InsertBookingAndUpdateTrainWithDates @PassengerName, @TrainNo, @Class, @DateOfTravel, @TotalSeats, @Totalamount OUTPUT",
                    PassengerNameParam, trainNoParam, classParam, dateOfTravelParam, totalSeatsParam, TotalamountParam);

                if (rowsAffected > 0)
                {
                    var bookTrain = db.Trains.FirstOrDefault(t => t.TrainNo == trainNo);
                    if (bookTrain != null)
                    {
                        if (ticketClass == "1A")
                            bookTrain.AvailableSeats -= totalSeats;
                        else if (ticketClass == "2A")
                            bookTrain.AvailableSeats -= totalSeats;
                        else if (ticketClass == "SL")
                            bookTrain.AvailableSeats -= totalSeats;
                        else if (ticketClass == "3A")
                            bookTrain.AvailableSeats -= totalSeats;

                        db.SaveChanges();
                    }
                }
                if (rowsAffected > 0)
                {
                    // Access the output parameter after executing the stored procedure
                    float totalAmount = Convert.ToSingle(TotalamountParam.Value);

                    var newBookingId = db.Bookings
                        .Where(b => b.PassengerName == customerName && b.TrainNo == trainNo && b.Class == ticketClass && b.Date_of_Travel == dateOfTravel)
                        .Select(b => b.Booking_ID)
                        .FirstOrDefault();

                    if (newBookingId != 0)
                    {
                        Console.WriteLine("                                                               ");
                        Console.WriteLine($"Ticket booked successfully! Your booking ID is: {newBookingId}");
                        Console.WriteLine("                                                               ");
                        Console.WriteLine($"Thank you for booking with us! We'll be here whenever you're ready for your next trip.");
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
            Console.WriteLine("                                                   ");
            Console.WriteLine("Enter your booking id:");
            int bookingId;
            if (!int.TryParse(Console.ReadLine(), out bookingId))
            {
                Console.WriteLine("Invalid input for booking ID.");
                return;
            }

            var BookedTickets = db.Bookings.Where(bt => bt.Booking_ID == bookingId).ToList();

            if (BookedTickets.Count == 0)
            {
                Console.WriteLine("No booked tickets found for the specified user.");
                return;
            }

            Console.WriteLine("Booked Tickets:");
            Console.WriteLine("-----------------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("| Train No  | Train Name       |  Passenger Name  |   Date of Travel   |   Class   |    Booking Date   |      Price          ");
            Console.WriteLine("-----------------------------------------------------------------------------------------------------------------------------");

            foreach (var ticket in BookedTickets)
            {
                var train = db.Trains.FirstOrDefault(sa => sa.TrainNo == ticket.TrainNo);

                Console.WriteLine($"| {ticket.TrainNo,-9} |   {train.TrainName,-13}  |   {ticket.PassengerName,-10}   | {ticket.Date_of_Travel,-15} | {ticket.Class,-6} | {ticket.BookingDate,-10} |{ticket.Totalamount}");
            }
            Console.WriteLine("------------------------------------------------------------------------------------------------------------------------------");


        }
        static void CancelBooking()
        {
            try
            {
                Console.WriteLine("                                                   ");
                Console.WriteLine("enter your booking id");
                int bookingId = Convert.ToInt32(Console.ReadLine());

                var booking = db.Bookings.FirstOrDefault(b => b.Booking_ID == bookingId);

                if (booking == null)
                {
                    Console.WriteLine("No booked ticket for cancel, Please book the tickets");
                    return;
                }
                else
                {
                    decimal refundamount = (decimal)booking.Totalamount * 0.75m;
                    var cancelledTicket = new Cancellation
                    {
                        Booking_ID = bookingId,
                        PassengerName = booking.PassengerName,
                        TrainNo = booking.TrainNo,
                        Class = booking.Class,
                        Cancel_Date = DateTime.Now,
                        No_of_Seats = booking.Totalseats,
                        Refund = (int?)refundamount
                    };
                    db.Cancellations.Add(cancelledTicket);
                    db.SaveChanges();

                    Console.WriteLine("Cancellation completed successfully.");

                    var cancelTrain = db.Trains.FirstOrDefault(t => t.TrainNo == booking.TrainNo);
                    if (cancelTrain != null)
                    {
                        if (booking.Class == "1A")
                            cancelTrain.AvailableSeats += (int)booking.Totalseats;
                        else if (booking.Class == "2A")
                            cancelTrain.AvailableSeats += (int)booking.Totalseats;
                        else if (booking.Class == "SL")
                            cancelTrain.AvailableSeats += (int)booking.Totalseats;
                        else if (booking.Class == "3A")
                            cancelTrain.AvailableSeats += (int)booking.Totalseats;

                        db.SaveChanges();
                    }
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error cancelling booking: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
            }
        }
        static void ShowCancelTickets()
        {
            Console.WriteLine("Presenting the details for the cancelled ticket-");
            Console.WriteLine("                                                  ");
            Console.WriteLine("Enter your booking id:");
            int bookingId = Convert.ToInt32(Console.ReadLine());
            var cancelledTickets = db.Cancellations.Where(t => t.Booking_ID == bookingId).ToList();

            Console.WriteLine($"Found {cancelledTickets.Count} cancelled tickets for booking ID: {bookingId}");

            if (cancelledTickets.Count == 0)
            {
                Console.WriteLine("No cancelled tickets found for the specified user.");
                return;
            }

            Console.WriteLine("Cancelled Tickets:");
            Console.WriteLine("-----------------------------------------------------------------------------------------------------------");
            Console.WriteLine("|Booking ID  |  Train No  |   Class |  Total Seats  |Cancellation Date       | Refund  |");
            Console.WriteLine("-----------------------------------------------------------------------------------------------------------");

            foreach (var ticket in cancelledTickets)
            {
                Console.WriteLine($"| {ticket.Booking_ID,-13} |        {ticket.TrainNo,-9} |     {ticket.Class,-9} |    {ticket.No_of_Seats} |     {ticket.Cancel_Date,-23} |   {ticket.Refund,-13} |");
            }

            Console.WriteLine("---------------------------------------------------------------------------------------------------------------->");
        }


        static void Admin()
        {
            int lineLength = Console.WindowWidth - "Welcome to the Admin menu".Length;
            int padding = lineLength / 3;
            string paddingStr = new string(' ', padding);
            Console.WriteLine(paddingStr + "***************************************************************************");
            Console.WriteLine(paddingStr + "|                                                                         |");
            Console.WriteLine(paddingStr + "|                    Welcome to the Admin menu                            |");
            Console.WriteLine(paddingStr + "|                                                                         |");
            Console.WriteLine(paddingStr + "***************************************************************************");
            Console.WriteLine("                                                                                                                 ");
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

                Console.WriteLine("Admin authentication successful..");
            }
            Console.WriteLine("----------------------------------------------------------------------------------------------------------------->");
            AdminMenu();

        }


        private static void AdminMenu()
        {
            Console.WriteLine("You've reached the Admin menu-");
            Console.WriteLine("                              ");
            while (true)
            {
                
                Console.WriteLine("Admin Menu:");
                Console.WriteLine("1. Add Train");
                Console.WriteLine("2. Modify Train");
                Console.WriteLine("3. Delete Train");
                Console.WriteLine("4. Reactivate Train");
                Console.WriteLine("5. User option");
                Console.WriteLine("6. Exit");

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
                    case 4:
                        ReactivateTrain();
                        break;
                    case 5:
                        User_option();
                        break;
                    case 6:
                        Environment.Exit(0);
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
            Console.WriteLine("                         ");
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
                return;
            }

            Console.Write("Arrival Time: ");
            TimeSpan arrivalTime;
            if (!TimeSpan.TryParse(Console.ReadLine(), out arrivalTime))
            {
                Console.WriteLine("Invalid arrival time format.");
                return;
            }

            // Assuming the train is active by default
            string status = "Active";

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
                Console.WriteLine("Train has been successfully added!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding train: {ex.Message}");
            }
            Console.WriteLine("---------------------------------------------------------------------------");
        }

        private static void ModifyTrain()
        {
            Console.WriteLine("                                                                ");
            Console.Write("Enter the Train No which you want to modify: ");
            int trainNo = Convert.ToInt32(Console.ReadLine());

            // Find the train with the provided train number
            var train = db.Trains.FirstOrDefault(t => t.TrainNo == trainNo);
            if (train == null)
            {
                Console.WriteLine("Train not found.");
                return;
            }

            // Display current details of the train
            Console.WriteLine("Current details of the train:");
            Console.WriteLine($"Train No: {train.TrainNo}");
            Console.WriteLine($"Train Name: {train.TrainName}");
            Console.WriteLine($"Source: {train.To}");
            Console.WriteLine($"Destination: {train.From}");
            Console.WriteLine($"Departure Time: {train.Departure_Time}");
            Console.WriteLine($"Arrival Time: {train.Arrival_Time}");
            Console.WriteLine($"status: {(train.Status == "yes" ? "Active" : "Inactive")}");

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

            Console.WriteLine("Train has been successfully modified!");
            Console.WriteLine("---------------------------------------------------------------------------------------------------------------->");
        }

        public static void DeleteTrain()
        {
            try
            {
                Console.WriteLine("                                                ");
                Console.Write("Enter Train No which you want to deactivate: ");
                int trainNo = Convert.ToInt32(Console.ReadLine());

                // Find all classes of the train in the database
                var trains = db.Trains.Where(t => t.TrainNo == trainNo).ToList();
                if (!trains.Any())
                {
                    Console.WriteLine("Train not found.");
                    Console.WriteLine("---------------------------------------------------------------------------");
                    return;
                }

                Console.WriteLine("Do you want to deactivate all classes of the train (y/n)");
                string confirm = Console.ReadLine();

                if (confirm.ToLower() == "y")
                {
                    foreach (var train in trains)
                    {
                        train.Status = "Inactive";
                    }

                    // Save changes to the database
                    db.SaveChanges();
                    Console.WriteLine("Train and all its classes deactivated successfully!");
                }
                else
                {
                    Console.WriteLine("Deactivation canceled.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deactivating train: {ex.Message}");

                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
            }

        }

        private static void ReactivateTrain()
        {
            try
            {
                Console.WriteLine("                                                ");
                Console.Write("Enter Train No of the train you want to reactivate: ");
                int trainNo = Convert.ToInt32(Console.ReadLine());

                // Find all classes of the train in the database
                var trains = db.Trains.Where(t => t.TrainNo == trainNo).ToList();
                if (!trains.Any())
                {
                    Console.WriteLine("Train not found.");
                    Console.WriteLine("---------------------------------------------------------------------------");
                    return;
                }

                Console.WriteLine("Do you want to reactivate all classes of the train (y/n)");
                string confirm = Console.ReadLine();

                if (confirm.ToLower() == "y")
                {
                    foreach (var train in trains)
                    {
                        train.Status = "active";
                    }

                    // Save changes to the database
                    db.SaveChanges();
                    Console.WriteLine("Train and all its classes reactivated successfully!");
                }
                else
                {
                    Console.WriteLine("Reactivation canceled.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reactivating train: {ex.Message}");

                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
            }

        }
    }
}
