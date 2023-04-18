/* Script to populate Cognitive Case Management database */


-- CaseType
INSERT INTO CaseType (Type, Description) VALUES ('FOIA Request', 'FOIA Request')
INSERT INTO CaseType (Type, Description) VALUES ('FOIA Consultation', 'FOIA Consultation')

select * from CaseType



-- Request
INSERT INTO Request (CaseNumber, CaseTypeId, RequestDate, ReceivedDate, Classification, RequestorId, RequesterOrganizationId, AssignedOfficerId, Background, Description)
VALUES ('FY 23-0001', 1, getdate(), getdate(), 'UNCLASSIFIED', 1, 1, 1, 'This is the background info.', 'This is the description.')

select * from Request


-- Requestor
INSERT INTO Requestor (FirstName, LastName, Email, Phone, RequesterOrganizationId)
VALUES ('Joseph', 'Jones', 'jj@wapo.com', '202-555-1212', 1)

-- Requestor Organization
INSERT INTO RequestorOrganization([Name], [Address]) VALUES ('Washington Post', '123 Wash Way, Washington DC 12345')


-- FOIA Officer
INSERT INTO FOIAOfficer(FirstName, LastName, Email, Phone, Title) VALUES ('Frank', 'Franklin', 'ff@centcom.mil', '505-555-1212', 'FOIA Officer')
