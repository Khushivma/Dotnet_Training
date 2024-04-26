--Creating the Database --
Create database TrainRerversationSystem;

use TrainRerversationSystem;

 -- Create Users table
CREATE TABLE Users (
    UserId INT PRIMARY KEY,
    UserName VARCHAR(50),
    Password VARCHAR(50)
);

insert into Users (UserId,UserName, Password) values (1,'Khushi', 'Pass123'),
(2,'users', '123');


-- Create AdminDetails table
CREATE TABLE AdminDetails (
    AdminId INT PRIMARY KEY,
    AdminName VARCHAR(50),
    Password VARCHAR(50)
);

-- Insert sample admin data
INSERT INTO AdminDetails (AdminId, AdminName, Password) VALUES (1, 'admin', '123');

-- Create stored procedure for user registration

CREATE PROCEDURE dbo.RegisterUser
    @Username NVARCHAR(50),
    @Password NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    -- Check if the username already exists
    IF EXISTS (SELECT 1 FROM Users WHERE Username = @Username)
    BEGIN
        -- Username already exists, raise error
        THROW 50001, 'Username already exists.', 1;
        RETURN;
    END

    -- Insert new user into the Users table
    INSERT INTO Users (Username, Password)
    VALUES (@Username, @Password);

    -- Return success
    SELECT 'New user registered successfully.' AS Result;
END

--User Login Stored Procedure:

CREATE PROCEDURE dbo.UserLogin
    @Username NVARCHAR(50),
    @Password NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    -- Check if the username and password match
    IF EXISTS (SELECT 1 FROM Users WHERE Username = @Username AND Password = @Password)
    BEGIN
        -- Username and password match, login successful
        SELECT 'User login successful.' AS Result;
    END
    ELSE
    BEGIN
        -- Username and password do not match, login failed
        THROW 50001, 'Invalid username or password.', 1;
        RETURN;
    END
END

drop TABLE Trains

CREATE TABLE Trains (
    TrainNo INT  ,
    TrainName VARCHAR(100) NOT NULL,
    Departure_Time TIME NOT NULL,
    Arrival_Time TIME NOT NULL,
    [From] VARCHAR(100) NOT NULL,
    [To] VARCHAR(100) NOT NULL,
	Class varchar(20),
    TotalSeats INT NOT NULL,
	AvailableSeats int not null,
	Price float not null,
	Status varchar(20) not null,
	PRIMARY KEY (TrainNo, Class)
);


--inserting trains data:
INSERT INTO Trains (TrainNo, TrainName, [From], [To], Departure_Time, Arrival_Time, Class, TotalSeats, AvailableSeats, Price, Status)
VALUES 
(20805, 'Andhra Pradesh Express', 'VISAKHAPATNAM', 'NEW Delhi', '22:00:00', '05:40:00', 'Sl', 400, 400, 900.00, 'Active'),
(20805, 'Andhra Pradesh Express', 'VISAKHAPATNAM', 'NEW Delhi', '22:00:00', '05:40:00', '3A', 100, 100, 1400.00, 'Active'),
(20805, 'Andhra Pradesh Express', 'VISAKHAPATNAM', 'NEW Delhi', '22:00:00', '05:40:00', '2A', 90, 90, 1800.00, 'Active'),
(20805, 'Andhra Pradesh Express', 'VISAKHAPATNAM', 'NEW Delhi', '22:00:00', '05:40:00', '1A', 80, 80, 2400.00, 'Active'),
(20504, 'Rajdhani Express', 'NEW Delhi', 'LUCKNOW', '11:45:00', '18:40:00', 'Sl', 500, 500, 650.00, 'Active'),
(20504, 'Rajdhani Express', 'NEW Delhi', 'LUCKNOW', '11:45:00', '18:40:00', '3A', 95, 95, 1100.00, 'Active'),
(20504, 'Rajdhani Express', 'NEW Delhi', 'LUCKNOW', '11:45:00', '18:40:00', '2A', 85, 85, 1500.00, 'Active'),
(20504, 'Rajdhani Express', 'NEW Delhi', 'LUCKNOW', '11:45:00', '18:40:00', '1A', 75, 75, 1900.00, 'Active'),
(20413, 'Kashi Mahakal Sf Express', 'VARANASI', 'KANPUR', '14:45:00', '20:45:00', 'Sl', 600, 600, 600.00, 'Active'),
(20413, 'Kashi Mahakal Sf Express', 'VARANASI', 'KANPUR', '14:45:00', '20:45:00', '3A', 120, 120, 1100.00, 'Active'),
(20413, 'Kashi Mahakal Sf Express', 'VARANASI', 'KANPUR', '14:45:00', '20:45:00', '2A', 100, 100, 1600.00, 'Active'),
(20413, 'Kashi Mahakal Sf Express', 'VARANASI', 'KANPUR', '14:45:00', '20:45:00', '1A', 90, 90, 2000.00, 'Active'),
(12504, 'HUMSAFAR Express', 'NEW Delhi', 'LUCKNOW', '11:45:00', '18:40:00', 'Sl', 300, 300, 700.00, 'Active'),
(12504, 'HUMSAFAR Express', 'NEW Delhi', 'LUCKNOW', '11:45:00', '18:40:00', '3A', 150, 150, 1200.00, 'Active'),
(12504, 'HUMSAFAR Express', 'NEW Delhi', 'LUCKNOW', '11:45:00', '18:40:00', '2A', 100, 100, 1700.00, 'Active'),
(12504, 'HUMSAFAR Express', 'NEW Delhi', 'LUCKNOW', '11:45:00', '18:40:00', '1A', 80, 80, 2500.00, 'Active'),
(20503, 'ARONAI EXPRESS', 'VISKHAPATNAM', 'CHENNAI', '15:45:00', '04:30:00', 'SL', 500, 500, 800.00, 'Active'),
(20503, 'ARONAI EXPRESS', 'VISKHAPATNAM', 'CHENNAI', '15:45:00', '04:30:00', '3A', 130, 130, 1200.00, 'Active'),
(20503, 'ARONAI EXPRESS', 'VISKHAPATNAM', 'CHENNAI', '15:45:00', '04:30:00', '2A', 95, 95, 1700.00, 'Active'),
(20503, 'ARONAI EXPRESS', 'VISKHAPATNAM', 'CHENNAI', '15:45:00', '04:30:00', '1A', 80, 80, 2000.00, 'Active');




--creating booking table :
drop table Bookings

CREATE TABLE Bookings (
	Booking_ID INT identity primary key,
	PassengerName varchar(30),
    TrainNo INT,
    Class VARCHAR(20),
	BookingDate datetime not null,
	Date_of_Travel datetime not null,
    Totalseats INT,
    Totalamount FLOAT,
	Status VARCHAR(20),
    FOREIGN KEY (TrainNo, Class) REFERENCES trains(TrainNo, Class)
);



drop procedure InsertBookingAndUpdateTrainWithDates 

CREATE OR ALTER PROCEDURE InsertBookingAndUpdateTrainWithDates
(
    @PassengerName VARCHAR(30),
    @TrainNo INT,
    @Class VARCHAR(20),
    @Date_of_Travel DATE,
    @Totalseats INT,
    @Totalamount FLOAT OUTPUT
)
AS
BEGIN
    DECLARE @Price FLOAT;
    DECLARE @AvailableSeats INT;
    DECLARE @TrainStatus VARCHAR(20);

    -- Get the fare, available seats, and status for the specified class
    SELECT @Price = t.Price, @AvailableSeats = t.AvailableSeats, @TrainStatus = t.Status
    FROM Trains t
    WHERE t.TrainNo = @TrainNo AND t.Class = @Class;

    -- Check if the train is active
    IF UPPER(@TrainStatus) = 'ACTIVE'
    BEGIN
        -- Check if there are enough available seats
        IF @AvailableSeats >= @Totalseats
        BEGIN
            -- Calculate total amount
            SET @Totalamount = @Price * @Totalseats;

            -- Insert booking details
            INSERT INTO Bookings (PassengerName, TrainNo, Class, BookingDate, Date_of_Travel, Totalseats, Totalamount, Status)
            VALUES (@PassengerName, @TrainNo, @Class, GETDATE(), @Date_of_Travel, @Totalseats, @Totalamount, 'booked');

            -- Get the ID of the inserted booking
            DECLARE @Booking_ID INT;
            SET @Booking_ID = SCOPE_IDENTITY();

            -- Update available seats in the trains table
            UPDATE Trains
            SET AvailableSeats = AvailableSeats - @Totalseats
            WHERE TrainNo = @TrainNo AND Class = @Class;

            PRINT 'Booking details inserted successfully!';
        END
        ELSE
        BEGIN
            PRINT 'Not enough available seats!';
        END;
    END
    ELSE
    BEGIN
        PRINT 'Booking failed. Please try when the train is in Active status.';
    END;
END; 


DECLARE @Booking_ID INT;
DECLARE @Totalamount FLOAT;

EXEC InsertBookingAndUpdateTrainWithDates 
    @PassengerName = 'John Doe',
    @TrainNo = 12345,
    @Class = 'First Class',
    @Date_of_Travel = '2024-04-16',
    @Totalseats = 2,
    @Totalamount = @Totalamount OUTPUT;

-- Output the results
SELECT  @Totalamount AS Total_Amount;



drop table Cancellations

CREATE TABLE Cancellations (
    Cancel_id INT identity primary key,
    Booking_ID INT,
	PassengerName varchar(30),
	TrainNo INT,
	Class varchar(20),
    Cancel_Date DATE NOT NULL,
	No_of_Seats INT,
	Refund INT,
	Remarks VARCHAR(20),
	FOREIGN KEY (TrainNo,Class) REFERENCES Trains(TrainNo,Class),
    FOREIGN KEY (Booking_ID) REFERENCES Bookings(Booking_ID)
);

drop  procedure InsertCancellationAndUpdateTrainWithRefund

CREATE OR ALTER PROCEDURE InsertCancellationAndUpdateTrainWithRefund (
    @Cancel_id INT,
    @Booking_ID INT,
	@PassengerName VARCHAR(30),
	@Class VARCHAR(20),
    @Cancel_Date DATE,
    @No_of_Seats INT,
    @Remarks VARCHAR(100) = ''
)
AS
BEGIN
    DECLARE @refund DECIMAL(10, 2);  -- Use decimal data type for currency

    BEGIN TRY
        DECLARE @Price FLOAT;
        DECLARE @Totalamount FLOAT;
        DECLARE @TrainNo INT;

        -- Get the Price, train number, and total amount paid for the cancelled booking
        SELECT @Price = t.Price, @TrainNo = b.TrainNo
        FROM Bookings b
        INNER JOIN Trains t ON b.TrainNo = t.TrainNo
        WHERE b.Booking_ID = @Booking_ID;

        -- Calculate total amount paid for the cancelled booking
        SET @Totalamount = @Price * @No_of_Seats;

        -- Calculate refund amount (75% refund)
        SET @refund = CAST(@Totalamount * 0.75 AS DECIMAL(10, 2)); -- 75% refund

        -- Insert cancellation details
        INSERT INTO Cancellations (Cancel_id, Booking_ID, PassengerName, TrainNo, Class, Cancel_Date, No_of_Seats, Refund, Remarks)
        VALUES (@Cancel_id, @Booking_ID, @PassengerName, @TrainNo, @Class, @Cancel_Date, @No_of_Seats, @refund, @Remarks);

        -- Update available seats in the trains table
        UPDATE Trains
        SET AvailableSeats = AvailableSeats + @No_of_Seats
        WHERE TrainNo = @TrainNo;

        PRINT 'Cancellation details inserted successfully!';
    END TRY
    BEGIN CATCH
        PRINT 'An error occurred during cancellation. Please try again later.';
    END CATCH;
END;


EXEC InsertCancellationAndUpdateTrainWithRefund 
    @Cancel_id = 1,
    @Booking_ID = 1001, 
	@PassengerName = 'Jeep',
	@Class = '3A',
    @Cancel_Date = '2024-04-16',
    @No_of_Seats = 2;

CREATE OR ALTER PROCEDURE UpdateTrainStatus
(
    @TrainNo INT,
    @NewStatus VARCHAR(20) -- "Active" or "Inactive"
)
AS
BEGIN
    UPDATE Trains
    SET Status = @NewStatus
    WHERE TrainNo = @TrainNo;
END;



select * from Trains

select * from Bookings


select * from Cancellations