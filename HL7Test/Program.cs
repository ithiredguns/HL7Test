// See https://aka.ms/new-console-template for more information
using System;
using System.Text;
using NHapi.Base.Parser;
using NHapi.Base.Model;
using NHapi.Model.V23.Message;
using NHapi.Base;
using NHapi.Base.Validation.Implementation;

Console.WriteLine("Hello, World  !");
// The full HL7 message string from the provided document.
        // Note: The Base64 PDF in the last OBX is truncated in the query, but in a real scenario, use the complete string.
        string hl7Message = @"
MSH|^~\&||DCLAB||1649487174|20241210000000||ORU^R01|0047|P|2.3|||||||||
PID|1|111111|34525325380|111111|DOE^JANE||19510518|F|||PO Box 1272^^Washington^CT^06793-||(212)555-5555|||||06008005^^^03^^F
PV1|1||||||1649487174^GOLDSTEIN^ANNE^|
ORC|RE|EVL869168^LAB|34525325380^LAB||||||202412110000|||1649487174^GOLDSTEIN^A^^^^^N
OBR|1|EVL869168^LAB|34525325380^LAB|005009^CBC With Differential/Platelet^L|||202412100000|||||||202412110000||||869168||EVL869168||202412111107|||F
OBX|1|NM|005025^WBC^L^6690-2^Leukocytes^LN||6.9|x10E3/uL|3.4-10.8|||N|F|20141201||202412110724|01|||||||||||||HE
OBX|2|NM|005033^RBC^L^789-8^Erythrocytes^LN||5.17|x10E6/uL|3.77-5.28|||N|F|20141201||202412110724|01|||||||||||||HE
OBX|3|NM|005041^Hemoglobin^L^718-7^Hemoglobin^LN||15.6|g/dL|11.1-15.9|||N|F|20141202||202412110724|01|||||||||||||HE
OBX|4|NM|005058^Hematocrit^L^4544-3^Hematocrit^LN||47.5|%|34.0-46.6|H||N|F|20141203||202412110724|01|||||||||||||HE
OBX|5|NM|015065^MCV^L^787-2^Erythrocyte mean corpuscular volume^LN||92|fL|79-97|||N|F|20241107||202412110724|01|||||||||||||HE
OBX|6|NM|015073^MCH^L^785-6^Erythrocyte mean corpuscular hemoglobin^LN||30.2|pg|26.6-33.0|||N|F|20241107||202412110724|01|||||||||||||HE
OBX|7|NM|015081^MCHC^L^786-4^Erythrocyte mean corpuscular hemoglobin concentrat^LN||32.8|g/dL|31.5-35.7|||N|F|20241107||202412110724|01|||||||||||||HE
OBX|8|NM|105007^RDW^L^788-0^Erythrocyte distribution width^LN||11.8|%|11.7-15.4|||N|F|20241107||202412110724|01|||||||||||||HE
OBX|9|NM|015172^Platelets^L^777-3^Platelets^LN||285|x10E3/uL|150-450|||N|F|20140813||202412110724|01|||||||||||||HE
OBX|10|NM|015107^Neutrophils^L^770-8^Neutrophils/100 leukocytes^LN||60|%|Not Estab.|||N|F|20241009||202412110724|01|||||||||||||HE
OBX|11|NM|015123^Lymphs^L^736-9^Lymphocytes/100 leukocytes^LN||25|%|Not Estab.|||N|F|20241009||202412110724|01|||||||||||||HE
OBX|12|NM|015131^Monocytes^L^5905-5^Monocytes/100 leukocytes^LN||10|%|Not Estab.|||N|F|20241009||202412110724|01|||||||||||||HE
OBX|13|NM|015149^Eos^L^713-8^Eosinophils/100 leukocytes^LN||4|%|Not Estab.|||N|F|20241009||202412110724|01|||||||||||||HE
OBX|14|NM|015156^Basos^L^706-2^Basophils/100 leukocytes^LN||1|%|Not Estab.|||N|F|20241009||202412110724|01|||||||||||||HE
OBX|15|NM|015909^Neutrophils (Absolute)^L^751-8^Neutrophils^LN||4.2|x10E3/uL|1.4-7.0|||N|F|20241112||202412110724|01|||||||||||||HE
OBX|16|NM|015917^Lymphs (Absolute)^L^731-0^Lymphocytes^LN||1.7|x10E3/uL|0.7-3.1|||N|F|20241112||202412110724|01|||||||||||||HE
OBX|17|NM|015925^Monocytes(Absolute)^L^742-7^Monocytes^LN||0.7|x10E3/uL|0.1-0.9|||N|F|20241112||202412110724|01|||||||||||||HE
OBX|18|NM|015933^Eos (Absolute)^L^711-2^Eosinophils^LN||0.3|x10E3/uL|0.0-0.4|||N|F|20241112||202412110724|01|||||||||||||HE
OBX|19|NM|015941^Baso (Absolute)^L^704-7^Basophils^LN||0.1|x10E3/uL|0.0-0.2|||N|F|20241107||202412110724|01|||||||||||||HE
OBX|20|NM|015108^Immature Granulocytes^L^71695-1^Granulocytes.immature/100 leukocytes^LN||0|%|Not Estab.|||N|F|20240930||202412110724|01|||||||||||||HE
OBX|21|NM|015911^Immature Grans (Abs)^L^53115-2^Granulocytes.immature^LN||0.0|x10E3/uL|0.0-0.1|||N|F|20240829||202412110724|01|||||||||||||HE
ORC|RE|EVL869168^LAB|34525325380^LAB||||||202412110000|||1649487174^GOLDSTEIN^A^^^^^N
OBR|2|EVL869168^LAB|34525325380^LAB|322000^Comp. Metabolic Panel (14)^L|||202412100000|||||||202412110000||||869168||EVL869168||202412111107|||F
OBX|1|NM|001032^Glucose^L^2345-7^Glucose^LN||206|mg/dL|70-99|H||N|F|||202412110906|01|||||||||||||AC
OBX|2|NM|001040^BUN^L^3094-0^Urea nitrogen^LN||27|mg/dL|8-27|||N|F|20241126||202412110908|01|||||||||||||AC
OBX|3|NM|001370^Creatinine^L^2160-0^Creatinine^LN||0.84|mg/dL|0.57-1.00|||N|F|20241126||202412110910|01|||||||||||||AC
OBX|4|NM|100779^eGFR^L^98979-8^Glomerular filtration rate/1.73 sq M.predicted^LN||73|mL/min/1.73|>59|||N|F|20230701||202412110910|01|||||||||||||AC
OBX|5|NM|011577^BUN/Creatinine Ratio^L^3097-3^Urea nitrogen/Creatinine^LN||32||12-28|H||N|F|20241107||202412110910|01|||||||||||||AC
OBX|6|NM|001198^Sodium^L^2951-2^Sodium^LN||140|mmol/L|134-144|||N|F|20241126||202412110905|01|||||||||||||AC
OBX|7|NM|001180^Potassium^L^2823-3^Potassium^LN||5.0|mmol/L|3.5-5.2|||N|F|20241126||202412110908|01|||||||||||||AC
OBX|8|NM|001206^Chloride^L^2075-0^Chloride^LN||103|mmol/L|96-106|||N|F|20241126||202412110905|01|||||||||||||AC
OBX|9|NM|001578^Carbon Dioxide, Total^L^2028-9^Carbon dioxide^LN||24|mmol/L|20-29|||N|F|20241126||202412110908|01|||||||||||||AC
OBX|10|NM|001016^Calcium^L^17861-6^Calcium^LN||10.2|mg/dL|8.7-10.3|||N|F|20241107||202412110906|01|||||||||||||AC
OBX|11|NM|001073^Protein, Total^L^2885-2^Protein^LN||6.8|g/dL|6.0-8.5|||N|F|20241126||202412110911|01|||||||||||||AC
OBX|12|NM|001081^Albumin^L^1751-7^Albumin^LN||4.5|g/dL|3.8-4.8|||N|F|20241126||202412110908|01|||||||||||||AC
OBX|13|NM|012039^Globulin, Total^L^10834-0^Globulin^LN||2.3|g/dL|1.5-4.5|||N|F|20241107||202412110911|01|||||||||||||AC
OBX|14|NM|001099^Bilirubin, Total^L^1975-2^Bilirubin^LN||0.2|mg/dL|0.0-1.2|||N|F|20241126||202412110911|01|||||||||||||AC
OBX|15|NM|001107^Alkaline Phosphatase^L^6768-6^Alkaline phosphatase^LN||84|IU/L|44-121|||N|F|20241126||202412110913|01|||||||||||||AC
OBX|16|NM|001123^AST (SGOT)^L^1920-8^Aspartate aminotransferase^LN||15|IU/L|0-40|||N|F|20241126||202412110910|01|||||||||||||AC
OBX|17|NM|001545^ALT (SGPT)^L^1742-6^Alanine aminotransferase^LN||16|IU/L|0-32|||N|F|20241126||202412110910|01|||||||||||||AC
ORC|RE|EVL869168^LAB|34525325380^LAB||||||202412110000|||1649487174^GOLDSTEIN^A^^^^^N
OBX|1|ED|PDFReport1^PDF Report1^L|000000|LCLS^Image^PDF^Base64^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... // (truncated Base64 PDF data - replace with full Base64 from the document)
||||||F||||01
";

        // Note: The provided HL7 has a structural issue (third ORC followed by OBX without OBR), which may cause parsing errors.
        // In a real HL7 message, ensure it conforms to the standard structure.
        string[] lines = hl7Message.Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.None);
        StringBuilder sb = new StringBuilder();
        foreach (string line in lines)
        {
            string trimmed = line.Trim();
            if (string.IsNullOrEmpty(trimmed)) continue; // Skip empty lines

            if (trimmed.Length >= 4 && char.IsUpper(trimmed[0]) && char.IsUpper(trimmed[1]) && char.IsUpper(trimmed[2]) && trimmed[3] == '|')
            {
                // New segment
                if (sb.Length > 0) sb.Append("\r");
                sb.Append(trimmed);
            }
            else
            {
                // Continuation of previous field (e.g., Base64 lines)
                sb.Append(trimmed);
            }
        }
        hl7Message = sb.ToString();

        PipeParser parser = new PipeParser();
        parser.ValidationContext = new DefaultValidation();

        try
        {
            //var segments = hl7Message.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
           // var result = new List<Dictionary<string, object>>();
            //string hl7MessageString = parser.Encode(new ADT_A01(){MSH = { MessageType = {  }}});
            IMessage parsedMessage = parser.Parse(hl7Message);
            ORU_R01 oruMessage = parsedMessage as ORU_R01;

            if (oruMessage != null)
            {
                Console.WriteLine("HL7 Message parsed successfully into ORU_R01 object.");

                // Example: Access MSH segment
                var msh = oruMessage.MSH;
                Console.WriteLine($"Message Type: {msh.MessageType.MessageType.Value}^{msh.MessageType.TriggerEvent.Value}");
                Console.WriteLine($"HL7 Version: {msh.VersionID.Value}");

                // Access the first (and typically only) PATIENT_RESULT group
                if (oruMessage.RESPONSERepetitionsUsed > 0)
                {
                    //var patientResult = oruMessage.GetPATIENT_RESULT(0);
                    var patientResult = oruMessage.RESPONSEs.FirstOrDefault();

                    // Example: Access PID segment (Patient Information)
                    var pid = patientResult.PATIENT.PID;
                    var patientName = pid.GetPatientName(0);
                    Console.WriteLine($"Patient Name: {patientName.GivenName.Value} {patientName.FamilyName.Value}");
                    Console.WriteLine($"Date of Birth: {pid.DateOfBirth.TimeOfAnEvent.Value}");
                    Console.WriteLine($"Gender: {pid.Sex.Value}");

                    // Example: Access PV1 segment (if present)
                    var pv1 = patientResult.PATIENT.VISIT.PV1;

                    var attendingDoctor = pv1.GetAttendingDoctor();
                    var admittingDoctor= pv1.GetAdmittingDoctor();
                    if (attendingDoctor != null && attendingDoctor.Any())
                    {
                        Console.WriteLine($"Attending Doctor: {pv1.GetAttendingDoctor().FirstOrDefault()?.IDNumber.Value} {pv1.GetAttendingDoctor().FirstOrDefault()?.GivenName.Value} {pv1.GetAttendingDoctor().FirstOrDefault()?.FamilyName.Value}");

                    }
                    
                    // Example: Loop through ORDER_OBSERVATION groups
                    for (int orderIndex = 0; orderIndex < patientResult.ORDER_OBSERVATIONRepetitionsUsed; orderIndex++)
                    {
                        var orderObs = patientResult.GetORDER_OBSERVATION(orderIndex);

                        // Access OBR (if present)
                        if (orderObs.OBR != null)
                        {
                           // Console.WriteLine($"Test Name: {orderObs.OBR..ObservationIdentifier.Text.Value}");
                           
                           var obs = orderObs.OBR;
                        }

                        // Loop through OBX segments
                        for (int obxIndex = 0; obxIndex < orderObs.OBSERVATIONRepetitionsUsed; obxIndex++)
                        {
                            var obx = orderObs.GetOBSERVATION(obxIndex).OBX;
                            var obsId = obx.ObservationIdentifier.Text.Value;
                            var valueType = obx.ValueType.Value;

                            Console.WriteLine($"OBX Identifier: {obsId} (Type: {valueType})");

                            if (valueType == "NM")
                            {
                                // Numeric value
                                var value = obx.GetObservationValue(0).Data.ToString();
                                Console.WriteLine($"Value: {value} {obx.Units.Identifier.Value}");
                            }
                            else if (valueType == "ED")
                            {
                                // Encapsulated Data (e.g., PDF)
                                var edValue = obx.GetObservationValue(0).Data as NHapi.Model.V23.Datatype.ED;
                                if (edValue != null)
                                {
                                    Console.WriteLine($"ED Type: {edValue.TypeOfData.Value}");
                                    Console.WriteLine($"ED Subtype: {edValue.DataSubtype.Value}");
                                    Console.WriteLine($"ED Encoding: {edValue.Encoding.Value}");
                                    string base64Data = edValue.Data.Value;
                                    Console.WriteLine($"Base64 PDF Data (preview): {base64Data.Substring(0, Math.Min(50, base64Data.Length))}...");

                                    // Optional: Decode Base64 to bytes (e.g., save as PDF file)
                                    try
                                    {
                                        byte[] pdfBytes = Convert.FromBase64String(base64Data);
                                        // System.IO.File.WriteAllBytes("output.pdf", pdfBytes);
                                        Console.WriteLine("PDF data decoded successfully.");
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine("PDF data decoded **UNSUCCESSFULLY**.");
                                        Console.WriteLine(e);
                                         
                                    }
                                    
                                }
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No PATIENT_RESULT group found.");
                }
            }
            else
            {
                Console.WriteLine("Failed to cast parsed message to ORU_R01.");
            }
        }
        catch (HL7Exception ex)
        {
            Console.WriteLine($"HL7 Parsing Error: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"General Error: {ex.Message}");
        }

        Console.ReadLine();