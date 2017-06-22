package ua.dp.isd.jrqs.tests;

import java.util.List;
import java.util.concurrent.TimeUnit;
import org.openqa.selenium.By;
import org.openqa.selenium.WebDriver;
import org.openqa.selenium.WebElement;
import org.openqa.selenium.ie.InternetExplorerDriver;

public class WebHelper {

    private static WebDriver driverInstance;
    private final int timeout = 10;
    
    public WebHelper() {
        if (driverInstance != null)
            return;
        driverInstance = new InternetExplorerDriver();
        setDefaultTimeout();
    }

    public WebDriver getDriverInstance() {
        return driverInstance;
    }

    public WebElement findElement(String cssSelector) {
        return driverInstance.findElement(By.cssSelector(cssSelector));
    }

    public List<WebElement> findElements(String cssSelector) {
        return driverInstance.findElements(By.cssSelector(cssSelector));
    }
    
    public void setDefaultTimeout() {
        driverInstance.manage().timeouts().implicitlyWait(timeout, TimeUnit.SECONDS);
    }
    
    public void removeTimeout() {
        driverInstance.manage().timeouts().implicitlyWait(0, TimeUnit.SECONDS);
    }
}
