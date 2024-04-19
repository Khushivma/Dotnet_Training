Create database TrainSystem

use TrainSystem

 -- Create Users table
CREATE TABLE Users (
    UserId INT PRIMARY KEY,
    UserName VARCHAR(50),
    Password VARCHAR(50)
);

insert into Users (UserId,UserName, Password) values (1,'Khushi', 'Pass123')

-- Create AdminDetails table
CREATE TABLE AdminDetails (
    AdminId INT PRIMARY KEY,
    AdminName VARCHAR(50),
    Password VARCHAR(50)
);

-- Insert sample admin data
INSERT INTO AdminDetails (AdminId, AdminName, Password) VALUES (1, 'admin', 'admin123');

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
)
select * from trains
--inserting trains data:
INSERT INTO trains (TrainNo, TrainName, [From], [To], Departure_Time, Arrival_Time , Class, TotalSeats,AvailableSeats,Price,Status)
VALUES (20805,'Andhra Pradesh Express','VISAKHAPATNAM', 'NEW Delhi','22:00:00', '05:40:00', '1A',500,500,4500.00,'active'),
(20805,'Andhra Pradesh Express','VISAKHAPATNAM', 'NEW Delhi','22:00:00', '05:40:00', 'Sl',800,800,900.00,'active'),
(20504, 'Rajdhani Express','NEW Delhi','LUCKNOW','11:45:00', '18:40:00', '2A',500,500,2000.00,'active'),
(20413, 'Kashi Mahakal Sf Express','VARANASI','KANPUR','14:45:00', '20:45:00', '1A',400,400,2500.00,'active'),
(12504, 'HUMSAFAR Express','NEW Delhi','LUCKNOW','11:45:00', '18:40:00', '3A',300,300,2000.00,'active'),
(20503, 'ARONAI EXPRESS','VISKHAPATNAM','CHENNAI','15:45:00', '04:30:00', 'Sl',600,600,800.00,'active')



--creating booking table :
drop table bookings

CREATE TABLE bookings (
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
)
select * from bookings

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

    -- Get the fare and available seats for the specified class
    SELECT @Price = Price, @AvailableSeats = AvailableSeats
    FROM Trains
    WHERE @TrainNo = @TrainNo AND Class = @Class;

    -- Check if there are enough available seats
    IF @AvailableSeats >= @Totalseats
    BEGIN
        -- Calculate total amount
        SET @Totalamount = @Price * @Totalseats;

        -- Insert booking details
        INSERT INTO bookings ( PassengerName,TrainNo, Class,BookingDate,Date_of_Travel, Totalseats,Totalamount,Status)
        VALUES ( @PassengerName, @TrainNo, @Class, getdate(),@Date_of_Travel, @Totalseats,@Totalamount,'booked');
		declare @Booking_ID int
		 SET @Booking_ID = SCOPE_IDENTITY();
        -- Update available seats in the trains table
        UPDATE Trains
        SET @AvailableSeats = AvailableSeats - @Totalseats
        WHERE TrainNo = @TrainNo AND Class = @Class;


		SELECT  @Totalamount AS Totalamount;
        PRINT 'Booking details inserted successfully!';
    END
    ELSE
    BEGIN
        PRINT 'Not enough available seats!';
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



drop table cancellations

CREATE TABLE cancellations (
    Cancel_id INT PRIMARY KEY,
    Booking_ID INT,
	PassengerName varchar(30),
	TrainNo INT,
	Class varchar(20),
    Cancel_Date DATE NOT NULL,
	No_of_Seats INT,
	Refund INT,
	Remarks VARCHAR(20),
	FOREIGN KEY (TrainNo,Class) REFERENCES Trains(TrainNo,Class),
    FOREIGN KEY (Booking_ID) REFERENCES bookings(Booking_ID)
)
select * from cancellations
drop  procedure InsertCancellationAndUpdateTrainWithRefund
create or alter procedure InsertCancellationAndUpdateTrainWithRefund (
    @Cancel_id INT,
    @Booking_ID INT,
	@PassengerName varchar(30),
	@Class VARCHAR(20),
    @Cancel_Date DATE,
    @No_of_Seats INT,
   @Remarks varchar(100)=''
)
AS
BEGIN
    DECLARE @refund float;
    DECLARE @Price FLOAT;
    DECLARE @Totalamount FLOAT;
    DECLARE @TrainNo INT;

    -- Get the Price, train number, and total amount paid for the cancelled booking
    SELECT @Price = t.Price, @TrainNo = b.TrainNo
    FROM bookings b
    INNER JOIN Trains t ON b.TrainNo = t.TrainNo
    WHERE b.Booking_ID = @Booking_ID;

    -- Calculate total amount paid for the cancelled booking
    SET @Totalamount = @Price * @No_of_Seats;

    -- Calculate refund amount (75% refund)
    SET @refund = CAST(@Totalamount * 0.75 AS INT); -- 75% refund

    -- Insert cancellation details
    INSERT INTO cancellations (Cancel_id,Booking_ID,PassengerName,TrainNo,Class,Cancel_Date,No_of_Seats,Refund,Remarks)
    VALUES ( @Cancel_id,@Booking_ID,@PassengerName, @TrainNo,@Class, @Cancel_Date, @No_of_Seats, @refund,'Refund is done via cash');

    -- Update available seats and refund in the trains table
    UPDATE Trains
    SET AvailableSeats  = AvailableSeats + @No_of_Seats
    WHERE @TrainNo = @TrainNo;

    PRINT 'Cancellation details inserted successfully!';
END;

EXEC InsertCancellationAndUpdateTrainWithRefund 
    @Cancel_id = 1,
    @Booking_ID = 1001, 
	@PassengerName = 'Jeep',
	@Class = '3A',
    @Cancel_Date = '2024-04-16',
    @No_of_Seats = 2;
    

