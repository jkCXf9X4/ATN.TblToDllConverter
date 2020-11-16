# Tbl to dll converter

Collects .tbl from specified folder
Any reference needed is resolved by using the output folder specified

## Usage

TblToDllConverter.exe /input_folder /output_folder /namespace

/input_folder: Folder with tbl files
/output_folder: full output path
/namespace: all libs that are merged are contained under this namespace

## Example

TblToDllConverter.exe C:/tlbfolder/ c:/output/ exampleNamespace

Depending on module access is then 
```c#
exampleNamespace.subnamespaces
```
