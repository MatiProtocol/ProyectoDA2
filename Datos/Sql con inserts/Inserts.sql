
-- SpecialEvents
INSERT INTO VirtualParkDb.dbo.SpecialEvents 
(Name, [Date], StartTime, EndTime, MaxCapacity, CurrentCapacity, AdditionalCost, Active)
VALUES
('Halloween Horror Night', '2025-10-31', '18:00', '23:00', 200, 0, 50.00, 1),
('Christmas Parade', '2025-12-24', '16:00', '20:00', 300, 0, 30.00, 1),
('Summer Music Festival', '2025-01-15', '14:00', '22:00', 500, 0, 40.00, 1);

-- Visitors
INSERT INTO VirtualParkDb.dbo.Visitors
(BirthDate, Name, Password, Email)
VALUES
('1990-05-15', 'Juan Pérez', 'pass123', 'juan.perez@email.com'),
('2005-08-20', 'Ana Gómez', 'secret', 'ana.gomez@email.com'),
('1985-02-10', 'Carlos Rodríguez', 'mypassword', 'carlos.rodriguez@email.com');

-- VisitorParkInformations
INSERT INTO VirtualParkDb.dbo.VisitorParkInformations
(Id, VisitorMembership, Points)
VALUES
(1, 'Standard', 120),
(2, 'Premium', 250),
(3, 'VIP', 500);

-- Workers
INSERT INTO VirtualParkDb.dbo.Workers
(Roles, Name, Password, Email)
VALUES
('ParkAdmin,', 'root', 'root', 'root@root.com'),
('Operator,', 'Laura Fernández', 'op123', 'laura.fernandez@email.com'),
('ParkAdmin,', 'Diego Martínez', 'admin456', 'diego.martinez@email.com'),
('Operator,ParkAdmin', 'Sofía López', 'opadmin789', 'sofia.lopez@email.com');

-- Attractions
INSERT INTO VirtualParkDb.dbo.Attractions
(Name, TypeAttraction, MinAge, MaxCapacity, Description, CurrentCapacity)
VALUES
('Montaña Rusa Extrema', 'RollerCoaster', 12, 24, 'Una montaña rusa con loops extremos', 0),
('Simulador Espacial', 'Simulator', 8, 15, 'Simulador de viaje espacial', 0),
('Espectáculo de Magia', 'Show', 0, 100, 'Show de magia en vivo', 0),
('Zona Interactiva Dino', 'InteractiveZone', 5, 50, 'Atracción educativa e interactiva', 0);

-- Tickets
INSERT INTO VirtualParkDb.dbo.Tickets
(VisitorParkInformationId, [Type], [Date], Consumed, SpecialEventId)
VALUES
(1, 'General', '2025-10-01', 0, NULL),
(2, 'SpecialEvent', '2025-10-31', 0, 1),
(3, 'General', '2025-12-24', 0, NULL),
(1, 'SpecialEvent', '2025-12-24', 0, 2);

-- VisitedAttractions
INSERT INTO VirtualParkDb.dbo.VisitedAttractions
(VisitorParkInformationId, AttractionId, VisitDate)
VALUES
(1, 1, '2025-10-02'),
(2, 2, '2025-10-31'),
(3, 3, '2025-12-24');
