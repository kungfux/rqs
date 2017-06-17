package ua.dp.isd.jrqs.tests;

import java.util.List;
import org.openqa.selenium.By;
import org.openqa.selenium.WebDriver;
import org.openqa.selenium.WebElement;

public class WebHelper {
    
    private WebDriver d;
    
    public WebHelper(WebDriver driver) {
        d = driver;
    }
    
    public WebElement findElement(String cssSelector) {
        return d.findElement(By.cssSelector(cssSelector));
    }

    public List<WebElement> findElements(String cssSelector) {
        return d.findElements(By.cssSelector(cssSelector));
    }
}
