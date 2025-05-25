
-- Insert sample users
INSERT INTO [User] (UserID, Username, PasswordHash, Role) VALUES
(1, 'admin01', 'hashed_password_1', 'Admin'),
(2, 'nurse01', 'hashed_password_2', 'Nurse'),
(3, 'parent01', 'hashed_password_3', 'Parent');

-- Insert sample parents
INSERT INTO Parent (ParentID, UserID, FullName, Gender, DateOfBirth, Address, Phone) VALUES
(1, 3, 'Nguyen Van A', 'M', '1980-05-20', '123 Le Loi, Q1, HCM', '0909123456');

-- Insert sample students
INSERT INTO Student (StudentID, FullName, Gender, DateOfBirth, Class, ParentID, UserID) VALUES
(1, 'Nguyen Thi B', 'F', '2012-09-15', '5A', 1, NULL);

-- Insert sample nurses
INSERT INTO Nurse (NurseID, FullName, Gender, DateOfBirth, Phone, UserID) VALUES
(1, 'Tran Thi C', 'F', '1985-04-10', '0912345678', 2);

-- Insert sample manager/admin
INSERT INTO ManagerAdmin (ManagerID, FullName, Gender, DateOfBirth, Address, UserID) VALUES
(1, 'Le Van D', 'M', '1975-08-01', '456 Nguyen Trai, Q5, HCM', 1);

-- Insert sample medical inventory
INSERT INTO MedicalInventory (ItemID, ItemName, Category, Quantity, Unit, Description) VALUES
(1, 'Paracetamol 500mg', 'Medicine', 100, 'viên', 'Thuốc hạ sốt'),
(2, 'Băng cá nhân', 'Supplies', 50, 'cuộn', 'Dùng để băng vết thương nhỏ');

-- Insert sample vaccination event
INSERT INTO VaccinationEvent (EventID, EventName, Date, Location, ManagerID) VALUES
(1, 'Tiêm chủng cúm học kỳ I', '2025-09-10', 'Phòng Y tế Trường', 1);

-- Insert sample vaccine record
INSERT INTO VaccineRecord (VaccineRecordID, StudentID, VaccinationEventID, NurseID, VaccineName, InjectionDate, Reaction, FollowUpStatus, InjectionSite) VALUES
(1, 1, 1, 1, 'Vắc-xin cúm', '2025-09-10', 'Không có', 'Ổn định', 'Cánh tay trái');

-- Insert sample health report
INSERT INTO HealthReport (ReportID, Date, Description, StudentID, NurseID) VALUES
(1, '2025-09-11', 'Kiểm tra thân nhiệt và thị lực định kỳ.', 1, 1);

-- 1. Tạo bảng Allergen để lưu danh sách chất gây dị ứng
CREATE TABLE Allergen (
    AllergenID INT PRIMARY KEY,
    AllergenName NVARCHAR(100) NOT NULL,
    Description TEXT
);

-- 2. Tạo bảng trung gian StudentAllergy để hỗ trợ mối quan hệ nhiều-nhiều
CREATE TABLE StudentAllergy (
    StudentID INT,
    AllergenID INT,
    Reaction TEXT,
    PRIMARY KEY (StudentID, AllergenID),
    FOREIGN KEY (StudentID) REFERENCES Student(StudentID),
    FOREIGN KEY (AllergenID) REFERENCES Allergen(AllergenID)
);

-- 3. Chuyển dữ liệu từ bảng Allergy sang Allergen và StudentAllergy
-- Thêm các chất gây dị ứng vào bảng Allergen (loại bỏ trùng lặp)
INSERT INTO Allergen (AllergenID, AllergenName, Description)
SELECT DISTINCT AllergyID, Allergen, NULL
FROM Allergy;

-- Chuyển dữ liệu từ Allergy sang StudentAllergy
INSERT INTO StudentAllergy (StudentID, AllergenID, Reaction)
SELECT RecordID, AllergyID, Reaction
FROM Allergy;

-- 4. Tạo trigger để đồng bộ trường Allergy trong HealthProfile
CREATE TRIGGER UpdateHealthProfileAllergy
ON StudentAllergy
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    UPDATE HealthProfile
    SET Allergy = (
        SELECT STRING_AGG(a.AllergenName, ', ')
        FROM StudentAllergy sa
        JOIN Allergen a ON sa.AllergenID = a.AllergenID
        WHERE sa.StudentID = HealthProfile.StudentID
    )
    FROM HealthProfile
    WHERE StudentID IN (SELECT StudentID FROM inserted UNION SELECT StudentID FROM deleted);
END;

-- 5. Cập nhật trường Allergy trong HealthProfile ban đầu
UPDATE HealthProfile
SET Allergy = (
    SELECT STRING_AGG(a.AllergenName, ', ')
    FROM StudentAllergy sa
    JOIN Allergen a ON sa.AllergenID = a.AllergenID
    WHERE sa.StudentID = HealthProfile.StudentID
);

-- 6. Xóa bảng Allergy cũ
DROP TABLE Allergy;
