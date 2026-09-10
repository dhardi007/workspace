IDENTIFICATION DIVISION.
       PROGRAM-ID. TESTCBL.
      * Hello from COBOL (GnuCOBOL + codelldb)
       DATA DIVISION.
       WORKING-STORAGE SECTION.
       01 SUM-VAL PIC 9(4) VALUE 0.
       01 I PIC 9(4).
       PROCEDURE DIVISION.
           DISPLAY "Hello from COBOL (GnuCOBOL + codelldb)".
           PERFORM VARYING I FROM 1 BY 1 UNTIL I > 5
               ADD I TO SUM-VAL
           END-PERFORM.
           DISPLAY "sum = " SUM-VAL.
           STOP RUN.