IF (EXISTS(SELECT * FROM [dbo].[Standard]))  
BEGIN  
    DELETE FROM [dbo].[Standard]  
END  

IF (EXISTS(SELECT * FROM [dbo].[Standard_Import]))  
BEGIN  
    DELETE FROM [dbo].[Standard_Import]  
END  
