extern "C"
{
    char* GetUserID()
    {
        NSString *uidString = @"";

        if ([[UIDevice currentDevice]respondsToSelector:@selector(identifierForVendor)]) //iOS6+
        {
            uidString = [NSString stringWithFormat: @"VENDOR-%@", [UIDevice currentDevice].identifierForVendor.UUIDString];
        }
        else
        {
            uidString = @"OLD";
        }
        
        const char* string = [uidString UTF8String];
        
        if (string == NULL)
            return NULL;
        
        char* res = (char*)malloc(strlen(string) + 1);
        strcpy(res, string);
        
        return res;
    }
}